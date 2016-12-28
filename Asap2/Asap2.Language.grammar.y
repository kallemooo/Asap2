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
            public ANNOTATION annotation;
            public ANNOTATION_TEXT annotation_text;
            public ADDR_EPK addr_epk;
            public ARRAY_SIZE array_size;
            public BIT_OPERATION bit_operation;
            public CALIBRATION_ACCESS calibration_access;
            public COMPU_VTAB compu_vtab;
            public COMPU_VTAB_RANGE compu_vtab_range;
            public MATRIX_DIM matrix_dim;
       }

%start main

%token <n> NUMBER
%token <d> DOUBLE
%token <s> QUOTED_STRING
%token <s> IF_DATA
%token <s> IDENTIFIER

%token <s> A2ML
%token A2ML_VERSION
%token ASAP2_VERSION
%token <alignment_token> ALIGNMENT
%token ADDR_EPK
%token ANNOTATION   
%token ANNOTATION_LABEL
%token ANNOTATION_ORIGIN
%token ANNOTATION_TEXT
%token ARRAY_SIZE
%token BIT_MASK
%token BIT_OPERATION
%token CALIBRATION_ACCESS
%token COMPU_VTAB
%token COMPU_VTAB_RANGE
%token CPU_TYPE
%token CUSTOMER
%token CUSTOMER_NO
%token DEFAULT_VALUE
%token DEPOSIT
%token DISPLAY_IDENTIFIER
%token RIGHT_SHIFT
%token LEFT_SHIFT
%token SIGN_EXTEND
%token MATRIX_DIM
%token PROJECT
%token HEADER
%token MODULE
%token MOD_COMMON
%token BYTE_ORDER
%token DATA_SIZE
%token VERSION
%token PROJECT_NO
%token MEASUREMENT
%token CHARACTERISTIC
%token ECU_ADDRESS
%token ECU_ADDRESS_EXTENSION
%token FORMAT

%token BEGIN
%token END

%type <deposit>             deposit
%type <byte_order>          byte_order
%type <data_size>           data_size
%type <s>                   project_no
%type <s>                   version
%type <a2ml>                a2ml
%type <addr_epk>            addr_epk
%type <alignment>           alignment
%type <annotation>          annotation
%type <annotation>          annotation_data
%type <annotation_text>     annotation_text
%type <annotation_text>     annotation_text_data
%type <array_size>          array_size
%type <bit_operation>       bit_operation
%type <bit_operation>       bit_operation_data
%type <calibration_access>  calibration_access
%type <compu_vtab>          compu_vtab
%type <compu_vtab>          compu_vtab_data
%type <compu_vtab_range>    compu_vtab_range
%type <compu_vtab_range>    compu_vtab_range_data
%type <matrix_dim>          matrix_dim
%type <ecu_address>         ecu_address
%type <ecu_address_ext>     ecu_address_extension
%type <format>              format
%type <mod_common>          mod_common
%type <mod_common>          mod_common_data
%type <module>              module
%type <module>              module_data
%type <project>             project
%type <project>             project_data
%type <header>              header
%type <header>              header_data
%type <measurement>         measurement
%type <measurement>         measurement_data
%type <version>             version
%type <if_data>             if_data

%%

main    : project
        | asap2_version project
        | asap2_version a2ml_version project
        ;


a2ml                        : A2ML {
                                $$ = new A2ML($1);
                            }
                            ;

a2ml_version                : A2ML_VERSION NUMBER NUMBER {
                                Asap2File.a2ml_version = new A2ML_VERSION((uint)$2, (uint)$3);
                            }
                            ;

asap2_version               : ASAP2_VERSION NUMBER NUMBER {
                                Asap2File.asap2_version = new ASAP2_VERSION((uint)$2, (uint)$3);
                            }
                            ;

addr_epk                    : ADDR_EPK NUMBER {
                                $$ = new ADDR_EPK((UInt64)$2);
                            }
                            ;

alignment                   : ALIGNMENT NUMBER {
                                $$ = new ALIGNMENT($1, (uint)$2);
                            }
                            ;

annotation                  : BEGIN ANNOTATION annotation_data END ANNOTATION {
                                $$ = $3;
                            }
                            ;

annotation_data             : /* empty */ {
                                $$ = new ANNOTATION();
                            }
                            | annotation_data ANNOTATION_LABEL QUOTED_STRING {
                                $$ = $1;
                                $$.annotation_label = new ANNOTATION_LABEL($3);
                            }
                            | annotation_data ANNOTATION_ORIGIN QUOTED_STRING {
                                $$ = $1;
                                $$.annotation_origin = new ANNOTATION_ORIGIN($3);
                            }
                            | annotation_data annotation_text {
                                $$ = $1;
                                $$.annotation_text = $2;
                            }
                            ;

annotation_text             : BEGIN ANNOTATION_TEXT annotation_text_data END ANNOTATION_TEXT {
                                $$ = $3;
                            }
                            ;

annotation_text_data        : /* empty */ {
                                $$ = new ANNOTATION_TEXT();
                            }
                            | annotation_text_data QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($$.data.Count.ToString(), new ANNOTATION_TEXT_DATA($2));
                            }
                            ;


array_size                  : ARRAY_SIZE NUMBER {
                                $$ = new ARRAY_SIZE((ulong)$2);
                            }
                            ;

bit_operation               : BEGIN BIT_OPERATION bit_operation_data END BIT_OPERATION {
                                $$ = $3;
                            }
                            ;

bit_operation_data          : /* empty */ {
                                $$ = new BIT_OPERATION();
                            }
                            | bit_operation_data RIGHT_SHIFT NUMBER {
                                $$ = $1;
                                $$.right_shift = new RIGHT_SHIFT((ulong)$3);
                            }
                            | bit_operation_data LEFT_SHIFT NUMBER  {
                                $$ = $1;
                                $$.left_shift = new LEFT_SHIFT((ulong)$3);
                            }
                            | bit_operation_data SIGN_EXTEND  {
                                $$ = $1;
                                $$.sign_extend = new SIGN_EXTEND();
                            }
                            ;


calibration_access          : CALIBRATION_ACCESS IDENTIFIER {
                                CALIBRATION_ACCESS.CALIBRATION_ACCESS_type access;  
                                try
                                {
                                    access = (CALIBRATION_ACCESS.CALIBRATION_ACCESS_type) Enum.Parse(typeof(CALIBRATION_ACCESS.CALIBRATION_ACCESS_type), $2);        
                                }
                                catch (ArgumentException)
                                {
                                    throw new Exception("Unknown CALIBRATION_ACCESS type: " + $2);
                                }
                                $$ = new CALIBRATION_ACCESS(access);
                            }
                            ;

compu_vtab                  : BEGIN COMPU_VTAB compu_vtab_data END COMPU_VTAB {
                                $$ = $3;
                            }
                            ;

compu_vtab_data             : IDENTIFIER QUOTED_STRING IDENTIFIER NUMBER {
                                $$ = new COMPU_VTAB(Name: $1, LongIdentifier: $2, NumberValuePairs: (uint)$4);
                                if ($3 != $$.ConversionType)
                                {
                                    throw new Exception("Unknown COMPU_VTAB ConversionType: " + $3);
                                }
                            }
                            | compu_vtab_data NUMBER QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($3, new COMPU_VTAB_DATA($2.ToString(), $3));
                            }
                            | compu_vtab_data DEFAULT_VALUE QUOTED_STRING {
                                $$ = $1;
                                $$.default_value = $3;
                            }
                            ;

compu_vtab_range            : BEGIN COMPU_VTAB_RANGE compu_vtab_range_data END COMPU_VTAB_RANGE {
                                $$ = $3;
                            }
                            ;

compu_vtab_range_data       : IDENTIFIER QUOTED_STRING NUMBER {
                                $$ = new COMPU_VTAB_RANGE(Name: $1, LongIdentifier: $2, NumberValueTriples: (uint)$3);
                            }
                            | compu_vtab_range_data NUMBER NUMBER QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($4, new COMPU_VTAB_RANGE_DATA((decimal)$2, (decimal)$3, $4));
                            }
                            | compu_vtab_range_data DOUBLE NUMBER QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($4, new COMPU_VTAB_RANGE_DATA((decimal)$2, (decimal)$3, $4));
                            }
                            | compu_vtab_range_data NUMBER DOUBLE QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($4, new COMPU_VTAB_RANGE_DATA((decimal)$2, (decimal)$3, $4));
                            }
                            | compu_vtab_range_data DOUBLE DOUBLE QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($4, new COMPU_VTAB_RANGE_DATA((decimal)$2, (decimal)$3, $4));
                            }
                            | compu_vtab_range_data DEFAULT_VALUE QUOTED_STRING {
                                $$ = $1;
                                $$.default_value = $3;
                            }
                            ;

project                     : BEGIN PROJECT project_data END PROJECT {
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

header          :   BEGIN HEADER header_data END HEADER {
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


project_no      :   PROJECT_NO IDENTIFIER   { $$ = $2; }
                |   PROJECT_NO NUMBER       { $$ = $2.ToString(); }
                ;

version         :   VERSION QUOTED_STRING   { $$ = new VERSION($2); }
                ;


module          :   BEGIN MODULE module_data END MODULE {                   
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
                | module_data compu_vtab {
                    $$ = $1;
                    $$.COMPU_VTABs.Add($2.Name, $2);
                }
                | module_data compu_vtab_range {
                    $$ = $1;
                    $$.COMPU_VTAB_RANGEs.Add($2.Name, $2);
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

mod_common_data :  QUOTED_STRING {
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


matrix_dim      : MATRIX_DIM NUMBER NUMBER NUMBER {
                    $$ = new MATRIX_DIM((uint)$2, (uint)$3, (uint)$4);
                }
                ;

measurement     : BEGIN MEASUREMENT measurement_data END MEASUREMENT {
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
                |  measurement_data annotation {
                    $$ = $1;
                    $$.annotation = $2;
                }
                |  measurement_data array_size {
                    $$ = $1;
                    $$.array_size = $2;
                }
                |  measurement_data BIT_MASK NUMBER {
                    $$ = $1;
                    $$.bit_mask = new BIT_MASK((ulong)$3);
                }
                |  measurement_data bit_operation {
                    $$ = $1;
                    $$.bit_operation = $2;
                }
                |  measurement_data matrix_dim {
                    $$ = $1;
                    $$.matrix_dim = $2;
                }
                |  measurement_data DISPLAY_IDENTIFIER IDENTIFIER {
                    $$ = $1;
                    $$.display_identifier = $3;
                }
                ;


deposit         : DEPOSIT IDENTIFIER {
                    DEPOSIT.DEPOSIT_type type;
                    try
                    {
                        type = (DEPOSIT.DEPOSIT_type) Enum.Parse(typeof(DEPOSIT.DEPOSIT_type), $2);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown DEPOSIT type: " + $2);
                    }
                    $$ = new DEPOSIT(type);
                }
                ;

byte_order      : BYTE_ORDER IDENTIFIER {
                    BYTE_ORDER.BYTE_ORDER_type order;   
                    try
                    {
                        order = (BYTE_ORDER.BYTE_ORDER_type) Enum.Parse(typeof(BYTE_ORDER.BYTE_ORDER_type), $2);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown BYTE_ORDER type: " + $2);
                    }
                    $$ = new BYTE_ORDER(order);
                }
                ;

data_size       : DATA_SIZE NUMBER {
                    $$ = new DATA_SIZE((uint)$2);
                }
                ;

ecu_address                 : ECU_ADDRESS NUMBER {
                                $$ = new ECU_ADDRESS((UInt64)$2);
                            }
                            ;

ecu_address_extension       : ECU_ADDRESS_EXTENSION NUMBER {
                                $$ = new ECU_ADDRESS_EXTENSION((UInt64)$2);
                            }
                            ;

format                      : FORMAT QUOTED_STRING {
                                $$ = new FORMAT($2);
                            }
                            ;

%%
