#define CSHARP
#define VC7

#region License
/*

MIT License

Copyright (c) 2021 MXPSQL

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
#endregion

/* uncomment this line to make this snek silent */
// #define SLBeep

using Towel;
using System;
using RNGGen;
using Kurukuru;
using System.IO;
// using BinIOUtils;
using Sharprompt;
using System.Media;
using Newtonsoft.Json;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using static System.AppDomain;
using System.Collections.Generic;

namespace SnekTheGam{

	[Serializable]
	class CatchExit : Exception
	{

	    public CatchExit()
	        : base(String.Format("Catch Bloc me for EZ Exit"))
	    {

	    }
	}

	[Tag("Main", "No One Change Me, I am always main because me entry point of pogram in this progrsm.")]
	class Program{

		// objects
		static char[] DirectionChars = { '^', 'v', '<', '>', };
		// static char[] DirectionChars = { '|', '|', '-', '-', };
		// static char[] DirectionChars = { '#', '#', '#', '#', };
		static char[] Objects = {'@', '+', 'A'};

		// sleeps
		static int sleep = 100;

		// console window
		static int width = Console.WindowWidth;
		static int height = Console.WindowHeight;

		// the map
		static Tile[,] map = new Tile[width, height];

		// snake
		static Direction? direction = null;
		static Queue<(int X, int Y)> snake = new();

		// positions
		static int hX = (width / 2), hY = (height / 2);
		static int X = hX, Y = hY;
		static bool closeRequested = false;
		static int GUIPos = (height - 2);

		// scores
		static int score = 0;
		static int highScore = 0;
		static int lstScore = 0;

		// Paths
		static string HomeDirName = "SnekTheGam";
		static string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		static string HomeDir = Path.Combine(new String[] {HomePath, HomeDirName});
		static string JSONPath = Path.Combine(new String[] {HomeDir, "Storage.json"});

		// eol
		static string eol = System.Environment.NewLine;

		// cli color
		static ConsoleColor cForeground = Console.ForegroundColor;
		static ConsoleColor cBackground = Console.BackgroundColor;

		// rng
		static RNGSys RS = new RNGSys();

		// static BinIO Bin = new BinIO();

		// timin
		static Stopwatch SurvivalTime = new Stopwatch();

		// path
		static string executable = CurrentDomain.FriendlyName, dllpath = System.Reflection.Assembly.GetEntryAssembly().Location; /*, exepath = Environment.GetCommandLineArgs()[0] CurrentDomain.BaseDirectory /* Application.ExecutablePath; */

		// audio play eh?
		static bool AudioRun = true;

		// debugger
		static bool IsDebugger = false;

		// static IntPtr ALDev = OpenTK.Audio.OpenAL.Alc.OpenDevice(null);

		// static OpenTK.ContextHandle ALCtx = OpenTk.Audio.OpenAl.Alc.CreateContext(ALDev, (int*)null);


		// spinners
		// static Spinner IOSpinner = new Spinner();

		// storage dictionary
		static Dictionary<string, string> JSONDict = new Dictionary<string, string>();

		// json related
		static JObject JSONTemp = new JObject(new JProperty("HighScore", 0),new JProperty("LastSessionScore", 0));
		static JSchema StorageSchema = JSchema.Parse(@"{
		  'type': 'object',
		  'properties': {
		    'HighScore': {'type': 'integer'},
		    'LastSessionScore': {'type': 'integer'}
		  },
		  'required': ['HighScore', 'LastSessionScore']
		}");

		static int Main(string[] args){

			// interupt handler
			Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

			// Debugger
			IsDebugger = System.Diagnostics.Debugger.IsAttached;

			// cli Parsing
			for(int itr = 0; itr < args.Length; itr++)
			{
				string arg = args[itr];

				if(arg == "--SLP" || arg == "-s"){

					string param;

					try{
						param = args[itr + 1];
					}
					catch(System.IndexOutOfRangeException)
					{
						Console.WriteLine("No parameters passed to " + arg + eol);
						return 1;
					}

					if(!int.TryParse(param, out sleep))
					{
						Console.Clear();
						Console.WriteLine("Invalid speed" + eol);
						return 1;
					}
				}
				else if(arg == "-h")
				{
					HalpScreen(executable, dllpath);
					return 0;
				}
				else if(arg == "-sl" || arg == "--SLN")
				{
					AudioRun = false;
				}
				else if(arg == "---Reset"){
					var eeh = Prompt.Confirm("Are you sure you want to reset your stored data?");
					if(eeh){
						Spinner.Start("Resseting data and writting to json...", () => {
							using(StreamWriter sw = new StreamWriter(JSONPath))
							using (JsonTextWriter writer = new JsonTextWriter(sw))
							{
								JObject jobj = new JObject(
									new JProperty("HighScore", 0),
									new JProperty("LastSessionScore", 0)
								);

								jobj.WriteTo(writer);
							}
						});
					}
					else{
						Console.WriteLine("That was close, let's imagine that this conversation didn't exist ");
					}

					return 0;
				}
			}


#if (!CSHARP)
	#error CSHARP IS NOT DEFINED, IS THIS EVEN A CSHARP PROJECT?
#endif

			if(!File.Exists(JSONPath)){
				Spinner.Start("there is no json file, let me make one at " + JSONPath, () => {
					using (StreamWriter swt = File.CreateText(JSONPath))
					using (JsonTextWriter writer = new JsonTextWriter(swt))
					{
						JSONTemp.WriteTo(writer);
					}
				});
			}

			// Console.SetCursorPosition(0, 0);
			try{
				Spinner.Start("hmmm.. Reading " + JSONPath, () => {
					// var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
					using(StreamReader srd = new StreamReader(JSONPath))
					{
						string content = srd.ReadToEnd();

						JObject testObj = JObject.Parse(content);

						bool jsonisvalid = testObj.IsValid(StorageSchema);

						if(!jsonisvalid){
							throw new Newtonsoft.Json.JsonReaderException("This json is invalid with the schema");
						}

						JSONDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

						highScore = Int32.Parse(JSONDict["HighScore"]);
						lstScore = Int32.Parse(JSONDict["LastSessionScore"]);

						// Console.WriteLine(JSONDict);
						// Console.WriteLine(JSONDict["HighScore"]);
						// Console.WriteLine(JSONDict["LastSessionScore"]);
						// Console.ReadLine();
					}
					/* using(JsonTextReader reader = new JsonTextReader(srd))
					{
						while (reader.Read()){
    						if (reader.Value != null)
    						{
    						    if(reader.TokenType ){
									Intreader.Value
								}
    						}
						}
					} */
				});
			}
			catch(Newtonsoft.Json.JsonReaderException jre){
				Prompt.Input<string>("Maam, I will cut the top of your sexy button up shirt because of " + jre.Message);
			}
			catch(IOException ioe){
				Prompt.Input<string>("Maam, there is this " + ioe.Message);

			}

			bool CatchXit_it = false;

			// prompt icon
			Prompt.Symbols.Prompt = new Symbol("🤔", "?");
			Prompt.Symbols.Done = new Symbol("😎", "V");
			Prompt.Symbols.Error = new Symbol("😱", "X");

			// parse toml

			// Console.ReadLine();

			// using(StreamReader reader = File.OpenText(Path.Combine(new String[]{HomeDir, "Config.toml"})))
			// {
			// 	try{
			//     	// Parse the table
			//     	TomlTable table = TOML.Parse(reader);
			// 	}
			// 	catch(TomlParseException tpe){
			// 		Console.WriteLine("What the hell man, is smth wrong with your toml?");
			// 		Console.WriteLine("BTW here it is");
			// 		foreach(TomlSyntaxException syntaxEx in tpe.SyntaxErrors)
        	// 			Console.WriteLine($"Error on {syntaxEx.Column}:{syntaxEx.Line}: {syntaxEx.Message}");
			// 	}
			// }

			// Console.WriteLine(HomeDir);
			// Console.ReadLine();

			// msys doesn't like this, so exit
			try{
				Console.Clear();
				Console.CursorVisible = false;
				Console.SetCursorPosition(0, 0);
				Console.CursorVisible = true;
			}
			catch(System.IO.IOException IOE)
			{
				Console.WriteLine("Window handle is invalid, cannot resume.");
				Console.WriteLine("Full message: " + eol + IOE);
				return 2;
			}

			// Run Run Run Gas Gas Gas
			try
			{

				// Alc.MakeContextCurrent(context);

				if(CatchXit_it == true)
				{
					throw new CatchExit();
				}
				
				Console.Title = "snek";
				Console.WriteLine("Snek the Game");
				Console.WriteLine("Your high score is " + highScore);
				Console.WriteLine("Your score from last session is " + lstScore);
				var eh = Prompt.Confirm("Are you ready to play the game?");
				if(!eh){
					throw new CatchExit();
				}

				bool resized = false;

				while(true){

					width = Console.WindowWidth;
					height = Console.WindowHeight;
					
					map = new Tile[width, height];
					hX = (width / 2); hY = (height / 2);
					GUIPos = (height - 2);
					X = hX; Y = hY;

					for(int i = 0; i < snake.Count; i++){
						try{
							snake.Dequeue();
						}
						catch(InvalidOperationException ioe){
							break;
						}
					}

					Console.CursorVisible = false;
					Console.Clear();
					snake.Enqueue((X, Y));
					map[X, Y] = Tile.Snake;
					PositionFood();
					PositionLaxative();
					Console.SetCursorPosition(X, Y);

					score = 0;

					closeRequested = false;

					Console.Write(Objects[0]);

					// Console.SetCursorPosition(0, GUIPos - 1);
					// Console.Write("Snek");

					// Console.Clear();

					direction = null;

					Console.BackgroundColor = ConsoleColor.Blue;
					for(int di = 0; di < width; di++)
					{
						Console.SetCursorPosition(di, GUIPos);
						Console.Write("#");
					}
					Console.BackgroundColor = cBackground;

					Console.SetCursorPosition(0, (GUIPos + 1));
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write("Press any key to start. Press escape to exit anytime you want.");
					Console.ForegroundColor = cForeground;
				
					while (!direction.HasValue && !closeRequested)
					{
						GetDirection();

						if (Console.WindowWidth != width || Console.WindowHeight != height)
						{
							// throw new CatchExit();
							resized = true;
						}
					}

					if(closeRequested)
					{
						Console.Clear();
						Console.CursorVisible = true;
						throw new CatchExit();
					}

					if (Console.WindowWidth != width || Console.WindowHeight != height)
					{
						resized = true;
					}

					Console.SetCursorPosition(0, (GUIPos + 1));
					Console.Write(new string(' ', Console.WindowWidth));
					while (!closeRequested)
					{
						if(resized){
							Console.Clear();
							Console.Write("Console was resized. Snake game has ended." + eol);
							Console.ReadLine();
							throw new CatchExit();
						}

						if (Console.WindowWidth != width || Console.WindowHeight != height)
						{
							resized = true;
						}
						switch (direction)
						{
							case Direction.Up: Y--; break;
							case Direction.Down: Y++; break;
							case Direction.Left: X--; break;
							case Direction.Right: X++; break;
						}

						score = (snake.Count - 1);

						eol = System.Environment.NewLine;

						if (X < 0 || X >= width ||
							Y < 0 || Y >= GUIPos ||
							map[X, Y] is Tile.Snake)
						{
							Console.Clear();
							Console.Write("Game Over. Score: " + score + "." + eol);
							Console.ReadLine();
							// throw new CatchExit();
							break;
						}
						Console.SetCursorPosition(X, Y);
						Console.Write(DirectionChars[(int)direction]);
						snake.Enqueue((X, Y));
						Console.SetCursorPosition(0, GUIPos);

						Console.BackgroundColor = ConsoleColor.Blue;
						for(int di = 0; di < width; di++)
						{
							Console.SetCursorPosition(di, GUIPos);
							Console.Write("#");
						}
						Console.BackgroundColor = cBackground;

						Console.SetCursorPosition(0, (GUIPos + 1));
						Console.ForegroundColor = ConsoleColor.DarkGreen;
						Console.Write("Yer score is: " + score);
						Console.ForegroundColor = cForeground;


						if (map[X, Y] == Tile.Food)
						{
							PositionFood();
							// PositionLaxative();
							if(RNGSys.RandomRandomNumber(1, 50) == 1){
								PositionLaxative();
							}
							Beep(AudioRun);
						}
						else
						{
							(int x, int y) = snake.Dequeue();
							map[x, y] = Tile.Open;
							Console.SetCursorPosition(x, y);
							Console.Write(' ');
						}

						Console.SetCursorPosition(X, Y);
						Console.Write(DirectionChars[(int)direction]);
						snake.Enqueue((X, Y));


						if(map[X, Y] == Tile.Laxative)
						{
							PositionLaxative();
							Console.Clear();
							Console.Write("Game Over. Score: " + score + "." + eol);
							Console.ReadLine();
							// throw new CatchExit();
							break;
						}
						else
						{
							(int x, int y) = snake.Dequeue();
							map[x, y] = Tile.Open;
							Console.SetCursorPosition(x, y);
							Console.Write(' ');
						}

						Console.SetCursorPosition(X, Y);
						Console.Write(DirectionChars[(int)direction]);
						snake.Enqueue((X, Y));
						snake.Dequeue();

						map[X, Y] = Tile.Snake;

						if (Console.KeyAvailable)
						{
							GetDirection();
						}

						if (Console.WindowWidth != width || Console.WindowHeight != height)
						{
							resized = true;
						}

						score = (snake.Count - 1);
						Thread.Sleep(sleep);
					}
					Console.Clear();


					eh = Prompt.Confirm("You want to play again?");
					if(!eh){
						throw new CatchExit();
					}
				}
			}
			catch(CatchExit ce)
			{
				lstScore = score;
				if(score > highScore){
					Console.WriteLine("Wahoo, you beat yer high score of " + highScore.ToString());
					highScore = score;
				}

				Spinner.Start("Writting to json...", () => {
					using(StreamWriter sw = new StreamWriter(JSONPath))
					using (JsonTextWriter writer = new JsonTextWriter(sw))
					{
						JObject jobj = new JObject(
							new JProperty("HighScore", highScore),
							new JProperty("LastSessionScore", lstScore)
						);

						jobj.WriteTo(writer);
					}
				});

				Beep(AudioRun);
				GC.Collect(0);
				GC.WaitForPendingFinalizers();
				GC.Collect(0);
				Console.ForegroundColor = cForeground;
				Console.BackgroundColor = cBackground;
				Console.CursorVisible = true;
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();

				dmsg(IsDebugger);

				// return 0;
			}
			catch(System.IO.IOException IOE)
			{
				Console.WriteLine("Window handle is invalid, cannot resume.");
				Console.WriteLine("Full message: " + eol + IOE);
			}
			catch(Exception e)
			{
				Console.WriteLine("An exception occured: " + e + eol);
			}
			finally
			{
				Beep(AudioRun);
				GC.Collect(0);
				GC.WaitForPendingFinalizers();
				GC.Collect(0);
				Console.ForegroundColor = cForeground;
				Console.BackgroundColor = cBackground;
				Console.CursorVisible = true;
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
			}

			dmsg(IsDebugger);

			return 0;
		}


		// this is obvious
		static void GetDirection()
		{
			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.UpArrow:    direction = Direction.Up; break;
				case ConsoleKey.DownArrow:  direction = Direction.Down; break;
				case ConsoleKey.LeftArrow:  direction = Direction.Left; break;
				case ConsoleKey.RightArrow: direction = Direction.Right; break;
				case ConsoleKey.Escape:     closeRequested = true; break;
			}
		}

		static void PositionFood()
		{
			List<(int X, int Y)> possibleCoordinates = new();
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < GUIPos; j++)
				{
					if (map[i, j] is Tile.Open)
					{
						possibleCoordinates.Add((i, j));
					}
				}
			}
			int index = RNGSys.NextRandomRandomNumber(possibleCoordinates.Count);
			(int X, int Y) = possibleCoordinates[index];
			map[X, Y] = Tile.Food;
			Console.SetCursorPosition(X, Y);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(Objects[1]);
			Console.ForegroundColor = cForeground;
		}

		static void PositionLaxative()
		{
			List<(int X, int Y)> possibleCoordinates = new();
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < GUIPos; j++)
				{
					if (map[i, j] is Tile.Open)
					{
						possibleCoordinates.Add((i, j));
					}
				}
			}
			int index = RNGSys.NextRandomRandomNumber(possibleCoordinates.Count);
			(int X, int Y) = possibleCoordinates[index];
			map[X, Y] = Tile.Laxative;
			Console.SetCursorPosition(X, Y);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(Objects[2]);
			Console.ForegroundColor = cForeground;
		}

		static void dmsg(bool run)
		{
			if(run)
			{
				// Console.WriteLine("\nPress <Enter> to continue...");
            	// Console.ReadLine();

				ConsoleHelper.PromptPressToContinue(null, ConsoleKey.Enter);
			}
		}

		enum Direction
		{
			Up = 0,
			Down = 1,
			Left = 2,
			Right = 3
		}

		enum Tile
		{
			Open = 0,
			Snake,
			Food,

			Laxative,
		}

		// the exit handler that we are talking about
		static void exitHandler(object sender, ConsoleCancelEventArgs args)
		{
			Console.Clear();
			Console.CursorVisible = true;
			Console.ForegroundColor = cForeground;
			Console.BackgroundColor = cBackground;
			// return 0;
		}

		// halp screen
		static int HalpScreen(string exe, string dlpath/* , string expath */)
		{
			Console.WriteLine("Snek CLI Usage: ");
			Console.WriteLine(exe + " <OPTIONS>");
			Console.WriteLine("dotnet " + dlpath + " <OPTIONS>");
			Console.WriteLine("");
			// Console.WriteLine(expath + " <OPTIONS>");
			Console.WriteLine("--SLP, -s\t\tsnake speed");
			Console.WriteLine("-sl, --SLN\t\tSilent");
			Console.WriteLine("---Reset\t\tReset your stored data");
			Console.WriteLine("-h\t\tdisplay this message");
			return 0;
		}

		static void Beep(bool run)
		{
#if !SLBeep
			if(run == true){
				Console.Beep();
			}
#endif
		}
	}
}