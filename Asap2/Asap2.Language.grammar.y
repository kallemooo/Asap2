%namespace Asap2
%partial
%parsertype Asap2Parser
%visibility internal
%tokentype Token

%union { 
			public int n;
			public string s;
			public HEADER header;
			public PROJECT project;
			public ASAP2_VERSION asap2_version;
	   }

%start main

%token <n> NUMBER
%token <s> QUOTED_STRING
%token <s> IDENTIFIER
%token <n> HEXNUMBER
%token ASAP2_VERSION
%token PROJECT
%token HEADER

%token BEGIN
%token END

%type <asap2_version>	asap2_version
%type <project>			project
%type <header>			header

%%

main	: project {
				Asap2File.project = $1;
			}
		| asap2_version project {
				Asap2File.asap2_version = $1;
				Asap2File.project = $2;
			}
		;

asap2_version	:   ASAP2_VERSION NUMBER NUMBER {
                        $$ = new ASAP2_VERSION((uint)$2, (uint)$3);
                    }
                ;

project			:	BEGIN PROJECT IDENTIFIER QUOTED_STRING header END PROJECT{
						$$ = new PROJECT($3, $4, $5);
					}
				|	BEGIN PROJECT IDENTIFIER QUOTED_STRING END PROJECT{
						$$ = new PROJECT($3, $4);
					}
				;

header			:	BEGIN HEADER QUOTED_STRING IDENTIFIER QUOTED_STRING IDENTIFIER IDENTIFIER END HEADER {
						$$ = new HEADER($3, $5, $7);
					}
				;

%%