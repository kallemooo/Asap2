%namespace Asap2
%partial
%parsertype Asap2Parser
%visibility internal
%tokentype Token

%union { 
            public decimal d;
            public String s;
            public StringBuilder sb;
            public ALIGNMENT.ALIGNMENT_type alignment_token;
            public ALIGNMENT alignment;
            public DEPOSIT deposit;
            public BYTE_ORDER byte_order;
            public MOD_COMMON mod_common;
            public MODULE module;
            public PROJECT project;
            public HEADER header;
            public MEASUREMENT measurement;
            public ECU_ADDRESS ecu_address;
            public ECU_ADDRESS_EXTENSION ecu_address_ext;
            public IF_DATA if_data;
            public A2ML a2ml;
            public ANNOTATION annotation;
            public ANNOTATION_TEXT annotation_text;
            public ADDR_EPK addr_epk;
            public ARRAY_SIZE array_size;
            public BIT_OPERATION bit_operation;
            public CALIBRATION_ACCESS calibration_access;
            public COMPU_TAB compu_tab;
            public COMPU_VTAB compu_vtab;
            public COMPU_VTAB_RANGE compu_vtab_range;
            public MATRIX_DIM matrix_dim;
            public MEMORY_SEGMENT memory_segment;
            public MEMORY_LAYOUT memory_layout;
            public MOD_PAR mod_par;
            public CALIBRATION_METHOD calibration_method;
            public CALIBRATION_HANDLE calibration_handle;
            public FUNCTION_LIST function_list;
            public MAX_REFRESH max_refresh;
            public SYMBOL_LINK symbol_link;
            public VIRTUAL Virtual;
            public GROUP group;
            public SUB_GROUP sub_group;
            public REF_CHARACTERISTIC ref_characteristic;
            public REF_MEASUREMENT ref_measurement;
}

%start main

%token <d> NUMBER
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
%token CALIBRATION_METHOD
%token CALIBRATION_HANDLE
%token CALIBRATION_HANDLE_TEXT
%token COMPU_TAB
%token COMPU_VTAB
%token COMPU_VTAB_RANGE
%token CPU_TYPE
%token CUSTOMER
%token CUSTOMER_NO
%token DEFAULT_VALUE
%token DEFAULT_VALUE_NUMERIC
%token DEPOSIT
%token DISPLAY_IDENTIFIER
%token DISCRETE
%token ECU
%token ECU_CALIBRATION_OFFSET
%token EPK
%token ERROR_MASK
%token RIGHT_SHIFT
%token LEFT_SHIFT
%token SIGN_EXTEND
%token MATRIX_DIM
%token PROJECT
%token HEADER
%token MODULE
%token MOD_COMMON
%token MOD_PAR
%token MEMORY_SEGMENT
%token MEMORY_LAYOUT
%token NO_OF_INTERFACES
%token BYTE_ORDER
%token DATA_SIZE
%token S_REC_LAYOUT
%token VERSION
%token PROJECT_NO
%token PHONE_NO
%token SUPPLIER
%token SYSTEM_CONSTANT
%token MEASUREMENT
%token CHARACTERISTIC
%token ECU_ADDRESS
%token ECU_ADDRESS_EXTENSION
%token FORMAT
%token LAYOUT
%token MAX_REFRESH
%token READ_WRITE
%token PHYS_UNIT
%token FUNCTION_LIST
%token USER
%token REF_MEMORY_SEGMENT
%token SYMBOL_LINK
%token VIRTUAL
%token GROUP
%token SUB_GROUP
%token REF_CHARACTERISTIC
%token REF_MEASUREMENT
%token ROOT
%token BEGIN
%token END

%type <deposit>             deposit
%type <byte_order>          byte_order
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
%type <calibration_method>  calibration_method
%type <calibration_method>  calibration_method_data
%type <calibration_handle>  calibration_handle
%type <calibration_handle>  calibration_handle_data
%type <compu_tab>           compu_tab
%type <compu_tab>           compu_tab_data
%type <compu_vtab>          compu_vtab
%type <compu_vtab>          compu_vtab_data
%type <compu_vtab_range>    compu_vtab_range
%type <compu_vtab_range>    compu_vtab_range_data
%type <matrix_dim>          matrix_dim
%type <ecu_address>         ecu_address
%type <ecu_address_ext>     ecu_address_extension
%type <function_list>       function_list
%type <function_list>       function_list_data
%type <mod_common>          mod_common
%type <mod_common>          mod_common_data
%type <mod_par>             mod_par
%type <mod_par>             mod_par_data
%type <module>              module
%type <module>              module_data
%type <memory_segment>      memory_segment
%type <memory_segment>      memory_segment_data
%type <memory_layout>       memory_layout
%type <memory_layout>       memory_layout_data
%type <project>             project
%type <project>             project_data
%type <header>              header
%type <header>              header_data
%type <measurement>         measurement
%type <measurement>         measurement_data
%type <max_refresh>         max_refresh
%type <symbol_link>         symbol_link
%type <if_data>             if_data
%type <Virtual>             Virtual
%type <Virtual>             Virtual_data
%type <group>               group
%type <group>               group_data
%type <sub_group>           sub_group
%type <sub_group>           sub_group_data
%type <ref_characteristic>  ref_characteristic
%type <ref_characteristic>  ref_characteristic_data
%type <ref_measurement>     ref_measurement
%type <ref_measurement>     ref_measurement_data
%type <s>                   default_value
%type <d>                   default_value_numeric

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
                                $$.data.Add($2);
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

                            
calibration_method          : BEGIN CALIBRATION_METHOD calibration_method_data END CALIBRATION_METHOD {
                                $$ = $3;
                            }
                            ;

calibration_method_data     : QUOTED_STRING NUMBER {
                                $$ = new CALIBRATION_METHOD($1, (ulong)$2);
                            }
                            | calibration_method_data calibration_handle {
                                $$ = $1;
                                $$.calibration_handle = $2;
                            }
                            ;

calibration_handle          : BEGIN CALIBRATION_HANDLE calibration_handle_data END CALIBRATION_HANDLE {
                                $$ = $3;
                            }
                            ;

calibration_handle_data     : NUMBER {
                                $$ = new CALIBRATION_HANDLE();
                                $$.Handles.Add((Int64)$1);
                            }
                            | calibration_handle_data NUMBER {
                                $$ = $1;
                                $$.Handles.Add((Int64)$2);
                            }
                            | calibration_handle_data CALIBRATION_HANDLE_TEXT QUOTED_STRING {
                                $$ = $1;
                                $$.text = $3;
                            }
                            ;

compu_tab                   : BEGIN COMPU_TAB compu_tab_data END COMPU_TAB {
                                $$ = $3;
                            }
                            ;

compu_tab_data              : IDENTIFIER QUOTED_STRING IDENTIFIER NUMBER {
                                COMPU_TAB.ConversionType conversionType;  
                                try
                                {
                                    conversionType = (COMPU_TAB.ConversionType) Enum.Parse(typeof(COMPU_TAB.ConversionType), $3);        
                                }
                                catch (ArgumentException)
                                {
                                    throw new Exception("Unknown COMPU_TAB ConversionType: " + $3);
                                }
                                $$ = new COMPU_TAB(Name: $1, LongIdentifier: $2, conversionType: conversionType, NumberValuePairs: (uint)$4);
                            }
                            | compu_tab_data NUMBER NUMBER {
                                $$ = $1;
                                $$.data.Add(new COMPU_TAB_DATA($2, $3));
                            }
                            | compu_tab_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
                            }
                            | compu_tab_data default_value_numeric {
                                $$ = $1;
                                $$.default_value_numeric = $2;
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
                                $$.data.Add(new COMPU_VTAB_DATA($2, $3));
                            }
                            | compu_vtab_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
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
                                $$.data.Add(new COMPU_VTAB_RANGE_DATA($2, $3, $4));
                            }
                            | compu_vtab_range_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
                            }
                            ;

default_value               : DEFAULT_VALUE QUOTED_STRING {
                                $$ = $2;
                            }
                            ;

default_value_numeric       : DEFAULT_VALUE_NUMERIC NUMBER {
                                $$ = $2;
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
                | header_data VERSION QUOTED_STRING {
                    $$ = $1;
                    $$.version = $3;
                }
                | header_data PROJECT_NO IDENTIFIER {
                    $$ = $1;
                    $$.project_no = $3;
                }
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
                    $$.A2MLs.Add($2);
                }
                | module_data compu_tab {
                    $$ = $1;
                    $$.COMPU_TABs.Add($2.Name, $2);
                }
                | module_data compu_vtab {
                    $$ = $1;
                    $$.COMPU_VTABs.Add($2.Name, $2);
                }
                | module_data compu_vtab_range {
                    $$ = $1;
                    $$.COMPU_VTAB_RANGEs.Add($2.Name, $2);
                }
                | module_data compu_vtab_range {
                    $$ = $1;
                    $$.COMPU_VTAB_RANGEs.Add($2.Name, $2);
                }
                | module_data group {
                    $$ = $1;
                    $$.groups.Add($2.GroupName, $2);
                }
                | module_data mod_par {
                    $$ = $1;
                    $$.mod_par = $2;
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
                |  mod_common_data DATA_SIZE NUMBER {
                    $$ = $1;
                    $$.data_size  = (UInt64)$3;
                }
                |  mod_common_data S_REC_LAYOUT IDENTIFIER {
                    $$ = $1;
                    $$.s_rec_layout  = $3;
                }
                |  mod_common_data alignment {
                    $$ = $1;
                    $$.alignments.Add($2.name, $2);
                }
                ;

mod_par         : BEGIN MOD_PAR mod_par_data END MOD_PAR {
                    $$ = $3;
                }
                ;

mod_par_data :  QUOTED_STRING {
                    $$ = new MOD_PAR($1);
                }
                |  mod_par_data addr_epk {
                    $$ = $1;
                    $$.addr_epk.Add($2);
                }
                |  mod_par_data calibration_method {
                    $$ = $1;
                    $$.calibration_method.Add($2);
                }
                |  mod_par_data CPU_TYPE QUOTED_STRING {
                    $$ = $1;
                    $$.cpu_type = $3;
                }
                |  mod_par_data CUSTOMER QUOTED_STRING {
                    $$ = $1;
                    $$.customer = $3;
                }
                |  mod_par_data CUSTOMER_NO QUOTED_STRING {
                    $$ = $1;
                    $$.customer_no = $3;
                }
                |  mod_par_data ECU QUOTED_STRING {
                    $$ = $1;
                    $$.ecu = $3;
                }
                |  mod_par_data ECU_CALIBRATION_OFFSET NUMBER {
                    $$ = $1;
                    $$.ecu_calibration_offset = (Int64)$3;
                }
                |  mod_par_data EPK QUOTED_STRING {
                    $$ = $1;
                    $$.epk = $3;
                }
                |  mod_par_data memory_layout {
                    $$ = $1;
                    $$.memory_layout.Add($2);
                }
                |  mod_par_data memory_segment {
                    $$ = $1;
                    $$.memory_segment.Add($2);
                }
                |  mod_par_data NO_OF_INTERFACES NUMBER {
                    $$ = $1;
                    $$.no_of_interfaces = (UInt64)$3;
                }
                |  mod_par_data PHONE_NO QUOTED_STRING {
                    $$ = $1;
                    $$.phone_no = $3;
                }
                |  mod_par_data SUPPLIER QUOTED_STRING {
                    $$ = $1;
                    $$.supplier = $3;
                }
                |  mod_par_data SYSTEM_CONSTANT QUOTED_STRING QUOTED_STRING {
                    $$ = $1;
                    $$.system_constants.Add($3, new SYSTEM_CONSTANT($3, $4));
                }
                |  mod_par_data USER QUOTED_STRING {
                    $$ = $1;
                    $$.user = $3;
                }
                |  mod_par_data VERSION QUOTED_STRING {
                    $$ = $1;
                    $$.version = $3;
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
                    DataType Datatype;  
                    try
                    {
                        Datatype = (DataType) Enum.Parse(typeof(DataType), $3);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown DataType: " + $3);
                    }                    

                    $$ = new MEASUREMENT($1, $2, Datatype, $4, (uint)$5, $6, $7, $8);
                }
                |  measurement_data annotation {
                    $$ = $1;
                    $$.annotation.Add($2);
                }
                |  measurement_data array_size {
                    $$ = $1;
                    $$.array_size = $2;
                }
                |  measurement_data BIT_MASK NUMBER {
                    $$ = $1;
                    $$.bit_mask = (UInt64)$3;
                }
                |  measurement_data bit_operation {
                    $$ = $1;
                    $$.bit_operation = $2;
                }
                |  measurement_data byte_order {
                    $$ = $1;
                    $$.byte_order = $2;
                }
                |  measurement_data DISCRETE {
                    $$ = $1;
                    $$.discrete = new DISCRETE();
                }
                |  measurement_data DISPLAY_IDENTIFIER IDENTIFIER {
                    $$ = $1;
                    $$.display_identifier = $3;
                }
                |  measurement_data ecu_address {
                    $$ = $1;
                    $$.ecu_address = $2;
                }
                |  measurement_data ecu_address_extension {
                    $$ = $1;
                    $$.ecu_address_extension = $2;
                }
                |  measurement_data ERROR_MASK NUMBER {
                    $$ = $1;
                    $$.error_mask = (UInt64)$3;
                }
                |  measurement_data FORMAT QUOTED_STRING {
                    $$ = $1;
                    $$.format = $3;
                }
                |  measurement_data function_list {
                    $$ = $1;
                    $$.function_list = $2;
                }
                |  measurement_data LAYOUT IDENTIFIER {
                    $$ = $1;
                    try
                    {
                        $$.layout = (MEASUREMENT.LAYOUT) Enum.Parse(typeof(MEASUREMENT.LAYOUT), $3);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown MEASUREMENT LAYOUT IndexMode: " + $3);
                    }                    
                }
                |  measurement_data matrix_dim {
                    $$ = $1;
                    $$.matrix_dim = $2;
                }
                |  measurement_data max_refresh {
                    $$ = $1;
                    $$.max_refresh = $2;
                }
                |  measurement_data PHYS_UNIT QUOTED_STRING {
                    $$ = $1;
                    $$.phys_unit = $3;
                }
                |  measurement_data READ_WRITE {
                    $$ = $1;
                    $$.read_write = new READ_WRITE();
                }
                |  measurement_data REF_MEMORY_SEGMENT IDENTIFIER {
                    $$ = $1;
                    $$.ref_memory_segment = $3;
                }
                |  measurement_data symbol_link {
                    $$ = $1;
                    $$.symbol_link = $2;
                }
                |  measurement_data Virtual {
                    $$ = $1;
                    $$.Virtual = $2;
                }
                | measurement_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
                }
                ;

max_refresh     : MAX_REFRESH NUMBER NUMBER {
                    $$ = new MAX_REFRESH((UInt64)$2, (UInt64)$3);
                }
                ;

symbol_link     : SYMBOL_LINK QUOTED_STRING NUMBER {
                    $$ = new SYMBOL_LINK($2, (UInt64)$3);
                }
                ;

function_list  : BEGIN FUNCTION_LIST function_list_data END FUNCTION_LIST {
                    $$ = $3;
                }
                ;

function_list_data  : /* start */  {
                    $$ = new FUNCTION_LIST();
                }
                |  function_list_data IDENTIFIER {
                    $$ = $1;
                    $$.functions.Add($2);
                }
                ;

Virtual         : BEGIN VIRTUAL Virtual_data END VIRTUAL {
                    $$ = $3;
                }
                ;

Virtual_data   : /* start */  {
                    $$ = new VIRTUAL();
                }
                |  Virtual_data IDENTIFIER {
                    $$ = $1;
                    $$.MeasuringChannel.Add($2);
                }
                ;

memory_segment  : BEGIN MEMORY_SEGMENT memory_segment_data END MEMORY_SEGMENT {
                    $$ = $3;
                }
                ;

memory_segment_data : IDENTIFIER QUOTED_STRING IDENTIFIER IDENTIFIER IDENTIFIER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
                    MEMORY_SEGMENT.PrgType PrgType;  
                    try
                    {
                        PrgType = (MEMORY_SEGMENT.PrgType) Enum.Parse(typeof(MEMORY_SEGMENT.PrgType), $3);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown MEMORY_SEGMENT PrgType: " + $3);
                    }                    

                    MEMORY_SEGMENT.MemoryType MemoryType;  
                    try
                    {
                        MemoryType = (MEMORY_SEGMENT.MemoryType) Enum.Parse(typeof(MEMORY_SEGMENT.MemoryType), $4);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown MEMORY_SEGMENT MemoryType: " + $4);
                    }                    

                    MEMORY_SEGMENT.Attribute Attribute;  
                    try
                    {
                        Attribute = (MEMORY_SEGMENT.Attribute) Enum.Parse(typeof(MEMORY_SEGMENT.Attribute), $5);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown MEMORY_SEGMENT Attribute: " + $5);
                    }                    

                    $$ = new MEMORY_SEGMENT($1, $2, PrgType, MemoryType, Attribute, (UInt64)$6, (UInt64)$7, (Int64)$8, (Int64)$9, (Int64)$10, (Int64)$11, (Int64)$12);
                }
                |  memory_segment_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
                }
                ;

memory_layout       : BEGIN MEMORY_LAYOUT memory_layout_data END MEMORY_LAYOUT {
                        $$ = $3;
                    }
                    ;

memory_layout_data  : IDENTIFIER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
                    MEMORY_LAYOUT.PrgType PrgType;  
                    try
                    {
                        PrgType = (MEMORY_LAYOUT.PrgType) Enum.Parse(typeof(MEMORY_LAYOUT.PrgType), $1);        
                    }
                    catch (ArgumentException)
                    {
                        throw new Exception("Unknown MEMORY_LAYOUT PrgType: " + $1);
                    }
                    $$ = new MEMORY_LAYOUT(PrgType, (UInt64)$2, (UInt64)$3, (Int64)$4, (Int64)$5, (Int64)$6, (Int64)$7, (Int64)$8);
                }
                |  memory_layout_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
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

ecu_address                 : ECU_ADDRESS NUMBER {
                                $$ = new ECU_ADDRESS((UInt64)$2);
                            }
                            ;

ecu_address_extension       : ECU_ADDRESS_EXTENSION NUMBER {
                                $$ = new ECU_ADDRESS_EXTENSION((UInt64)$2);
                            }
                            ;

group                       : BEGIN GROUP group_data END GROUP {
                                $$ = $3;
                            }
                            ;

group_data                  : IDENTIFIER QUOTED_STRING {
                                $$ = new GROUP($1, $2);
                            }
                            |  group_data annotation {
                                $$ = $1;
                                $$.annotation.Add($2);
                            }
                            | group_data if_data {
                                $$ = $1;
                                $$.if_data.Add($2);
                            }
                            |  group_data function_list {
                                $$ = $1;
                                $$.function_list = $2;
                            }
                            |  group_data ref_characteristic {
                                $$ = $1;
                                $$.ref_characteristic = $2;
                            }
                            |  group_data ref_measurement {
                                $$ = $1;
                                $$.ref_measurement = $2;
                            }
                            |  group_data ROOT {
                                $$ = $1;
                                $$.root = new ROOT();
                            }
                            |  group_data sub_group {
                                $$ = $1;
                                $$.sub_group = $2;
                            }
                            ;

ref_characteristic          : BEGIN REF_CHARACTERISTIC ref_characteristic_data END REF_CHARACTERISTIC {
                                $$ = $3;
                            }
                            ;

ref_characteristic_data     : /* start */  {
                                $$ = new REF_CHARACTERISTIC();
                            }
                            |  ref_characteristic_data IDENTIFIER {
                                $$ = $1;
                                $$.reference.Add($2);
                            }
                            ;

ref_measurement             : BEGIN REF_MEASUREMENT ref_measurement_data END REF_MEASUREMENT {
                                $$ = $3;
                            }
                            ;

ref_measurement_data        : /* start */  {
                                $$ = new REF_MEASUREMENT();
                            }
                            |  ref_measurement_data IDENTIFIER {
                                $$ = $1;
                                $$.reference.Add($2);
                            }
                            ;

sub_group                   : BEGIN SUB_GROUP sub_group_data END SUB_GROUP {
                                $$ = $3;
                            }
                            ;

sub_group_data              : /* start */  {
                                $$ = new SUB_GROUP();
                            }
                            |  sub_group_data IDENTIFIER {
                                $$ = $1;
                                $$.groups.Add($2);
                            }
                            ;
%%
