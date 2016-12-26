%namespace Asap2
%partial
%parsertype Asap2Parser
%visibility internal
%tokentype Token

%union { 
			public long n;
			public double d;
			public string s;
			public ALIGNMENT.ALIGNMENT_type alignment_token;
			public ALIGNMENT alignment;
			public DEPOSIT deposit;
			public BYTE_ORDER byte_order;
			public MOD_COMMON mod_common;
			public MODULE module;
			public PROJECT project;
			public HEADER header;
			public MEASUREMENT measurement;
			public VERSION version;
			public DATA_SIZE data_size;
			public ECU_ADDRESS ecu_address;
			public ECU_ADDRESS_EXTENSION ecu_address_ext;
			public FORMAT format;
			public IF_DATA if_data;
			public A2ML a2ml;
	   }

%start main

%token <n> NUMBER
%token <d> DOUBLE
%token <s> QUOTED_STRING
%token <s> IF_DATA
%token <s> A2ML
%token <s> IDENTIFIER
%token ASAP2_VERSION
%token A2ML_VERSION
%token PROJECT
%token HEADER
%token MODULE
%token MOD_COMMON
%token DEPOSIT
%token BYTE_ORDER
%token DATA_SIZE
%token VERSION
%token PROJECT_NO
%token MEASUREMENT
%token CHARACTERISTIC
%token ECU_ADDRESS
%token ECU_ADDRESS_EXTENSION
%token FORMAT
%token <alignment_token> ALIGNMENT

%token BEGIN
%token END

%type <deposit>			deposit
%type <byte_order>		byte_order
%type <data_size>		data_size
%type <s>				project_no
%type <s>				version
%type <alignment>       alignment
%type <ecu_address>		ecu_address
%type <ecu_address_ext> ecu_address_extension
%type <format>			format
%type <mod_common>      mod_common
%type <mod_common>      mod_common_data
%type <module>          module
%type <module>          module_data
%type <project>         project
%type <project>         project_data
%type <header>          header
%type <header>          header_data
%type <measurement>     measurement
%type <measurement>     measurement_data
%type <version>         version
%type <if_data>         if_data
%type <a2ml>            a2ml

%%

main	: project
		| asap2_version project
		| asap2_version a2ml_version project
		;

asap2_version	:   ASAP2_VERSION NUMBER NUMBER {
                        Asap2File.asap2_version = new ASAP2_VERSION((uint)$2, (uint)$3);
                    }
                ;

a2ml_version	:   A2ML_VERSION NUMBER NUMBER {
                        Asap2File.a2ml_version = new A2ML_VERSION((uint)$2, (uint)$3);
                    }
                ;

project			:	BEGIN PROJECT project_data END PROJECT {
					$$ = $3;
					Asap2File.project = $3;
				}
				;

project_data    :   IDENTIFIER QUOTED_STRING {
					$$ = new PROJECT();
					$$.name           = $1;
					$$.LongIdentifier = $2;
				}
				| project_data header {
					$$ = $1;
					$$.header = $2;
				}
				| project_data module {
					$$ = $1;
					$$.modules.Add($2.name, $2);
				}
				;

header			:	BEGIN HEADER header_data END HEADER {
						$$ = $3;
					}
				;
				
header_data     : QUOTED_STRING {
					$$ = new HEADER();
					$$.longIdentifier = $1;
				}
				| header_data version {
					$$ = $1;
					$$.version    = $2;
				}
				| header_data project_no {
					$$ = $1;
					$$.project_no = new PROJECT_NO($2);
				}
				;


project_no		:	PROJECT_NO IDENTIFIER	{ $$ = $2; }
				|	PROJECT_NO NUMBER		{ $$ = $2.ToString(); }
				;

version			:	VERSION QUOTED_STRING	{ $$ = new VERSION($2); }
				;


module			:	BEGIN MODULE module_data END MODULE {					
					$$ = $3;
				}
				;

module_data :   IDENTIFIER QUOTED_STRING {
					$$ = new MODULE();
					$$.name = $1;
					$$.LongIdentifier = $2;
				}
                | module_data mod_common {
					$$ = $1;
					$$.mod_common = $2;
				}
                | module_data measurement {
					$$ = $1;
                    $$.measurements.Add($2.name, $2);
                }
                | module_data if_data {
					$$ = $1;
                    $$.IF_DATAs.Add($2.name, $2);
                }
                | module_data a2ml {
					$$ = $1;
                    $$.A2MLs.Add($$.A2MLs.Count.ToString(), $2);
                }
				;

a2ml      : A2ML {
					$$ = new A2ML($1);
				}
				;

if_data      : IF_DATA {
					$$ = new IF_DATA($1);
				}
				;

mod_common      : BEGIN MOD_COMMON mod_common_data END MOD_COMMON {
					$$ = $3;
				}
				;

mod_common_data	:  QUOTED_STRING {
					$$ = new MOD_COMMON($1);
				}
                |  mod_common_data deposit {
					$$ = $1;
					$$.deposit    = $2;
				}
                |  mod_common_data byte_order {
					$$ = $1;
					$$.byte_order = $2;
				}
                |  mod_common_data data_size {
					$$ = $1;
					$$.data_size  = $2;
				}
				|  mod_common_data alignment {
					$$ = $1;
					$$.alignments.Add($2.name, $2);
				}
				;



measurement		: BEGIN MEASUREMENT measurement_data END MEASUREMENT {
					$$ = $3;
				}
				;

measurement_data :  IDENTIFIER QUOTED_STRING IDENTIFIER IDENTIFIER NUMBER NUMBER NUMBER NUMBER {
					$$ = new MEASUREMENT($1, $2, $3, $4, (uint)$5, (uint)$6, (uint)$7, (uint)$8);
				}
				|  measurement_data ecu_address {
					$$ = $1;
					$$.ecu_address = $2;
				}
                |  measurement_data ecu_address_extension {
					$$ = $1;
					$$.ecu_address_extension = $2;
				}
                |  measurement_data format {
					$$ = $1;
					$$.format = $2;
				}
				;


alignment		:   ALIGNMENT NUMBER {
                        $$ = new ALIGNMENT($1, (uint)$2);
                    }
                ;

deposit			: DEPOSIT IDENTIFIER {
					DEPOSIT.DEPOSIT_type type;
					switch ($2)
					{
						case "ABSOLUTE":
							type = DEPOSIT.DEPOSIT_type.ABSOLUTE;
						break;
						case "DIFFERENCE":
							type = DEPOSIT.DEPOSIT_type.DIFFERENCE;
						break;
						default:
						throw new Exception("Unknown DEPOSIT type: " + $2);
					}
					$$ = new DEPOSIT(type);
				}
                ;

byte_order		: BYTE_ORDER IDENTIFIER {
					BYTE_ORDER.BYTE_ORDER_type order;
					switch ($2)
					{
						case "LITTLE_ENDIAN":
							order = BYTE_ORDER.BYTE_ORDER_type.LITTLE_ENDIAN;
						break;
						case "BIG_ENDIAN":
							order = BYTE_ORDER.BYTE_ORDER_type.BIG_ENDIAN;
						break;
						case "MSB_FIRST":
							order = BYTE_ORDER.BYTE_ORDER_type.MSB_FIRST;
						break;
						case "MSB_LAST":
							order = BYTE_ORDER.BYTE_ORDER_type.MSB_LAST;
						break;
						default:
						throw new Exception("Unknown BYTE_ORDER type: " + $2);
					}
					$$ = new BYTE_ORDER(order);
				}
                ;

data_size		: DATA_SIZE NUMBER {
                    $$ = new DATA_SIZE((uint)$2);
                }
				;

ecu_address					: ECU_ADDRESS NUMBER {
								$$ = new ECU_ADDRESS((UInt64)$2);
							}
							;

ecu_address_extension		: ECU_ADDRESS_EXTENSION NUMBER {
								$$ = new ECU_ADDRESS_EXTENSION((UInt64)$2);
							}
							;

format						: FORMAT QUOTED_STRING {
								$$ = new FORMAT($2);
							}
							;

%%
