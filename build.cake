//////////////////////////////////////////////////////////////////////
// CAKE SCRIPT or CEK SCRIPT
//////////////////////////////////////////////////////////////////////



/* a clock is here

                                                                                          
                                                                                          
                                                                                          
                                   ....:::......:::::...                                  
                             .......::--====+++===---:...:::..                            
                         ..:..:-=+*######%%%%%%%%%######*+=-::-::.                        
                      .:...-+####%%%@@@@@@@@@@@@@@@@@@@%%####*=::--:                      
                    .:..-+###%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%###*=-:--.                   
                 .::.:+###%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%##*=:-=:                 
                ::.-*##%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%##*=--=.               
              .-.-*##%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#%@@@@@@@%##*=-=-              
             -::+##%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#: :%@@@@@@@@%##*-=+:            
           .=::*##@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@= .-#@@@@@@@@@@@%##==+-           
          .=:-*#%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%: :=%@@@@@@@@@@@@@%##+=+=          
         .=:=##%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@*..=*%@@@@@@@@@@@@@@@%##*=+=         
         =-=##%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%= -+%@@@@@@@@@@@@@@@@@@%##*=*:        
        ==-###@@@@@@@@@@@@@@@@@@@%++%@@@@@@@@@@@@@#:.=*%@@@@@@@@@@@@@@@@@@@@%##++*        
       .=-+##@@@@@@@@@@@@@@@@@@@@#-..=#%%%%%%%%%%+.:=*%@@@@@@@@@@@@@@@@@@@@@@##*+*-       
       ===##%@@@@@@@@@@@@@@@@@@%%%#+-. :*%%%%%%%-.=*#%%%%%%%@@@@@@@@@@@@@@@@@@#*++*       
       +-+*#@@@@@@@@@@@@@@@%%%%%%%%%#+=: :*%%%#:.+#%%%%%%%%%%%%@@@@@@@@@@@@@@@#**+*-      
      :+-**#@@@@@@@@@@@@%%%%%%%%%%%%%%#+=::=#*==*#%%%%%%%%%%%%%%%%@@@@@@@@@@@@%**+*+      
      -+-**%@@@@@@@@@@%%%%%%%%%%%%%%%%%%##=::..=%%%%%%%%%%%%%%%%%%%%%@@@@@@@@@%**++#      
      -+-**%@@@@@@@@%%%%%%%%%%%%%%%%%%%%%@:.-:.-%%%%%%%%%%%%%%%%%%%%%%%@@@@@@@@**++#      
      -+-**#@@@@@@%%%%%%%%%%%%%%%%%%%%%%%%%**+=*%%%%%%%%%%%%%%%%%%%%%%%%@@@@@@%**+**      
      .+=+*#@@@@@%%%%%%%%%%%%%%%%%%%%%%%%%%#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%@@@@%**+*+      
       +==**@@@%%%%%%%%%%%%%%%%%%%%%%%%%%%#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%@@@#+++*:      
       -+=++#@%%%%%%%%%%%%%%%%%%%%%%%%%%%##%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%@%++++*       
        +==++%%%%%%%%%%%%%%%%%%%%%%%%%%%%#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*+++*:       
        -+=+++%%%%%%%%%%%%%%%%%%%%%%%%%%#%%#######%%%%%%%%%%%%%%%%%%%%%%%%%%*+++++        
         +==++*%%%%%%%%%%%%%%%%%%%%%%####%###########%%%%%%%%%%%%%%%%%%%%%%#++=+*.        
          +===+*%%%%%%%%%%%%%%%%%%%#####%##############%%%%%%%%%%%%%%%%%%%#===+*:         
           ++===+%%%%%%%%%%%%%%%%%#####%################%%%%%%%%%%%%%%%%%*===+*:          
            ++====#%%%%%%%%%%%%%%########################%%%%%%%%%%%%%%%+===**.           
             :+====+#%%%%%%%%%%%######%###################%%%%%%%%%%%%*====*=             
              .=+=-==+#%%%%%%%%%#####%####################%%%%%%%%%#+=--=+*:              
                :*+=---=*%%%%%%%%###%#####################%%%%%%%#+---=+*=                
                  -++=----+*#%%%%########################%%%%%*+=---=+*=.                 
                   .:=*+--::-=+*#%######################%#*+=-::--+*+-.                   
               ........=+*+=-:::::-=+*##############*++=:::::--+**=:.......               
              ........::::=+**=--::......::::::::.......:--=**+=-:::.........             
              ........:::::--=+*****+==--::::::::---=+******+--:::::........              
                 ........::::--==++**################**++==--::::........                 
                       .........:::::-------==-------:::::..........                      
                                     .................                                    
                                                                                          


/* and cake is here

             .-  ===                                                                      
             -*+.#***=:                                                                   
             =****#*****:-.                                                               
           :-=***###****  .-=                                                             
         -=.   :=+*****...::--                                                            
       =-:..:::::::****-::::--                                                            
       *:::::::---=#*******#+-                                                            
       #+*****#####%%****####                                                             
       ############%@%****###                                                             
       ############%@@%***###                                                             
       ############%@@@#****#                                                             
       #####***++===+#@@#****=                                                            
       :..            .=*****+                                                            

*/


using System;

var target = Argument("target", "Test");
var configuration_production = Argument("configuration", "Release");
var configuration = configuration_production;
var configuration_debug = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Setup(context =>
{
    // Executed BEFORE the first task.
    Console.WriteLine(@"
                   --=-.                               
            *##=   #***#*:                             
           .#**#-  =#******=.                          
            *#**##**##******#*:.                       
           .*#***#***#********#*=++-                   
           +#******#####*******#  .-*+:                
        :++-+#*****######*****#*     .=#=              
     .-+=.   :+*##**********##-        .*+             
   :++:         .:-++*******%-::::::::::=#             
.=+=.  ............:::*******#-:::::::::=#             
#+::::::::::::::::::::*********-::::::::=#             
#=::::::::::::::::::::*#*******#++****#+=#             
#=:::::::::::----===++#%#*******%######++*             
#+===+++*****#########%@%#*******%#######.             
######################%@@%#*******%######.             
######################%@@@%#*******%#####.             
######################%@@@@%#*******%####.             
######################%@@@@@%#*******%###.             
######################%@@@@@@%#*******%%#.             
######################%@@@@@@@%#*******%#.             
######################%@@@@@@@@%#*******#:             
##################**+==-=*%@@@@@%#*******#:            
#%##*++=--:...             :+%@@@%#*******#            
                              :+*%%##****#+            
                                  .:-=++=:             

");
    Console.WriteLine("Cek build builds a snek");
});


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Debug").Does(() =>
{
    DotNetCoreBuild("./snek.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration_debug,
    });
});

Task("Build-Production").Does(() =>
{
    DotNetCoreBuild("./snek.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build-Production")
    .Does(() =>
{
    DotNetCoreTest("./snek.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);