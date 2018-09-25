

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudokuFungerer
{
	class MainClass
	{

		static void Main(string[] args)
		{

			//Array som inneholder alle verdier
			int[,] array = new int[9, 9];
			int choice = 0;
			while (choice == 0)
			{
				System.Console.SetCursorPosition(0, 0);
				System.Console.Write("Trykk <1> for et forhåndslagd sudokuproblem.");
				System.Console.SetCursorPosition(0, 1);
				System.Console.Write("Trykk <2> for å lage ditt eget sudokuproblem.");
				System.Console.SetCursorPosition(0, 2);
				System.Console.Write("Trykk <Enter> etter du har skrevet inn ditt valg.");

				choice = Convert.ToInt32(System.Console.ReadLine());
				System.Console.SetCursorPosition(0, 16);
				System.Console.Write(choice);
			}
			if (choice == 1)
			{
				array = new int[,]
					   {{ 0,7,5, 4,0,0,  0,3,0},
						{ 0,0,6, 0,3,0,  0,0,0},
						{ 0,0,2, 0,0,0,  0,0,1},

						{ 0,9,0, 0,8,7,  0,0,0},
						{ 3,0,0, 2,0,6,  0,0,7},
						{ 0,0,0, 5,1,0,  0,4,0},

						{ 5,0,0, 0,0,0,  4,0,0},
						{ 0,0,0, 0,7,0,  3,0,0},
						{ 0,1,0, 0,0,4,  6,7,0}
				};
			}
			else
			{
				System.Console.SetCursorPosition(0, 4);
				System.Console.Write("Fyll sudokumatrisen med verdier fra 1-9, hvis ikke verdien er fra 1-9, blir ingen verdi satt inn.");
				System.Console.SetCursorPosition(0, 5);
				System.Console.Write("Trykk <Enter> etter du har skrevet inn ditt valg.");
				for (int i = 0; i < 9; i++)
				{
					for (int y = 0; y < 9; y++)
					{
						System.Console.SetCursorPosition(0, 6);
						System.Console.Write("Rad: " + (i + 1) + ". Kolonne: " + (y + 1) + ": ");


						int num = Convert.ToInt32(System.Console.ReadLine());
						if (num > 0 && num < 10)
						{
							array[i, y] = num;
						}
						else
						{
							array[i, y] = 0;
						}
					}
				}
			}

			display2dArray(array);

			//Trigger for start av løsning

			System.Console.SetCursorPosition(0, 7);
			System.Console.Write("Trykk <Enter> for å starte sudokuløseren.");
			System.Console.ReadKey();

			SudokuSolver(0, 0, array);

			System.Console.ReadKey();


		}

		// !!!NB for å bruke denne må vi lage en funksjon som sjekker slik at ingen tall kan være like nedover/oppover/bortover osv

		//Fyller med genererte verdier og returnerer et 2d array
		private static int[,] generateSudokuArray()
		{
			Random rand = new Random();
			int[,] array = new int[9, 9];
			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
				{
					array[x, y] = 0;
					if (rand.Next(1, 4) % 3 == 0)
					{
						while (array[x, y] == 0)
						{
							int num = rand.Next(1, 10);
							if (checkall(x, num, y, array))
							{
								array[x, y] = num;
								break;
							}
						}
					}
				}

			}
			return array;
		}

		//Skriver ut et 2d array i form for sudokubrett
		private static void display2dArray(int[,] array)
		{
			int tabsLeft = 0;
			int tabsTop = 10;
			for (int i = 0; i < Math.Sqrt(array.Length); i++)
			{
				tabsLeft = 0;
				for (int y = 0; y < Math.Sqrt(array.Length); y++)
				{
					Console.SetCursorPosition(y + tabsLeft, i + tabsTop);
					Console.Write(array[i, y]);
					if ((y + 1) % 3 == 0)
					{
						tabsLeft += 2;
					}
				}
				if ((i + 1) % 3 == 0)
				{
					tabsTop++;
				}

			}
		}

		//Hovedmetode for løsning, går gjennom funksjoner og finner ut hvilken verdig som kan bli lagt til i rad og kolonne.
		private static Boolean SudokuSolver(int row, int col, int[,] array)
		{
			//hvis rader er større enn 8
			if (row > 8)
			{
				// Console.Write("funnet løsning 1");
				System.Console.SetCursorPosition(0, 15);
				return true;
				//throw new Exception("Løsning funnet");

			}
			if (array[row, col] != 0)
			{
				//Console.Write("prøver å flytte i solve");
				return Move(row, col, array);

			}
			else
			{
				for (int num = 1; num <= 9; num++)
				{
					if (checkall(row, num, col, array))
					{
						array[row, col] = num;
						if (Move(row, col, array))
						{
							display2dArray(array);

							//Console.Write("Her går det galt");
							return true;
						}
					}
				}
			}
			array[row, col] = 0;
			return false;
		}

		//Sjekker raden om "num" allerede eksisterer
		private static Boolean checkall(int row, int num, int col, int[,] array)
		{
			for (int i = 0; i < 9; ++i)
			{
				if (array[row, i] == num)
				{
					return false;
				}
			}
			//Sjekker kolonnene om "num" eksisterer fra før av.
			//Boolean isUnique = true;
			for (int i = 0; i < 9; i++)
			{
				if (array[i, col] == num)
				{
					return false;
				}
			}
			//Sjekker feltene rundt [row, col] om num eksisterer allerede.
			//Lager matrixen
			int rowTim = (row / 3) * 3;
			int colTim = (col / 3) * 3;

			for (int i = 0; i < 3; ++i)
			{
				for (int y = 0; y < 3; ++y)
				{
					if (num == array[rowTim + i, colTim + y])
					{
						return false;
					}
				}
			}
			//Trekkene fungerer i matrixen
			return true;
		}

		//Går til neste rad og kolonne, og starter enda en solve() rekursivt
		private static Boolean Move(int row, int col, int[,] array)
		{
			if (col < 8)
			{
				return SudokuSolver(row, col + 1, array);
			}
			else
			{
				return SudokuSolver(row + 1, 0, array);
			}
		}
	}
}