%namespace Asap2
%partial
%parsertype Asap2Parser
%visibility internal
%tokentype Token

%union { 
			public int n;
			public string s;
			public ALIGNMENT_type alignment_token;
			public Tuple<ALIGNMENT_type, uint> alignment;
			public DEPOSIT deposit;
			public BYTE_ORDER byte_order;
			public MOD_COMMON mod_common;
			public MODULE module;
			public PROJECT project;
			public HEADER header;
			public MEASUREMENT measurement;
	   }

%start main

%token <n> NUMBER
%token <s> QUOTED_STRING
%token <s> IDENTIFIER
%token <n> HEXNUMBER
%token ASAP2_VERSION
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
%type <n>				data_size
%type <s>				project_no
%type <s>				version
%type <alignment>       alignment
%type <n>				ecu_address
%type <n>				ecu_address_extension
%type <s>				format
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

%%

main	: project
		| asap2_version project
		| asap2_version project header
		| any
		| main any
		;

any    :  module
       ;

asap2_version	:   ASAP2_VERSION NUMBER NUMBER {
                        Asap2File.asap2_version = new ASAP2_VERSION((uint)$2, (uint)$3);
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
					$$.project_no = $2;
				}
				;


project_no		:	PROJECT_NO IDENTIFIER	{ $$ = $2; }
				|	PROJECT_NO NUMBER		{ $$ = $2.ToString(); }
				|	PROJECT_NO HEXNUMBER	{ $$ = $2.ToString(); }
				;

version			:	VERSION QUOTED_STRING	{ $$ = $2; }
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
					$$.data_size  = (uint)$2;
				}
				|  mod_common_data alignment {
					$$ = $1;
					$$.alignments.Add($2.Item1, $2.Item2);
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
					Console.WriteLine("Found ecu_address");
					$$ = $1;
					$$.ECU_ADDRESS = (UInt64)$2;
				}
                |  measurement_data ecu_address_extension {
					Console.WriteLine("Found ecu_address_extension");
					$$ = $1;
					$$.ECU_ADDRESS_EXTENSION = (UInt64)$2;
				}
                |  measurement_data format {
					Console.WriteLine("Found format");
					$$ = $1;
					$$.FORMAT = $2;
				}
				;


alignment		:   ALIGNMENT NUMBER {
                        $$ = new Tuple<ALIGNMENT_type, uint>($1, (uint)$2);
                    }
                ;

deposit			: DEPOSIT IDENTIFIER {
					switch ($2)
					{
						case "ABSOLUTE":
							$$ = DEPOSIT.ABSOLUTE;
						break;
						case "DIFFERENCE":
							$$ = DEPOSIT.DIFFERENCE;
						break;
						default:
						throw new Exception("Unknown DEPOSIT type: " + $2);
					}
				}
                ;

byte_order		: BYTE_ORDER IDENTIFIER {
					switch ($2)
					{
						case "MSB_FIRST":
							$$ = BYTE_ORDER.MSB_FIRST;
						break;
						case "MSB_LAST":
							$$ = BYTE_ORDER.MSB_LAST;
						break;
						default:
						throw new Exception("Unknown BYTE_ORDER type: " + $2);
					}
				}
                ;

data_size		: DATA_SIZE HEXNUMBER {
                    $$ = $2;
                }
				| DATA_SIZE NUMBER {
                    $$ = $2;
                }
				;

ecu_address					: ECU_ADDRESS HEXNUMBER {
								$$ = $2;
							}
							;

ecu_address_extension		: ECU_ADDRESS_EXTENSION HEXNUMBER {
								$$ = $2;
							}
							;

format						: FORMAT QUOTED_STRING {
								$$ = $2;
							}
							;

%%
private MODULE currentModule;
private MEASUREMENT currentMeasurment;
