using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class FieldGen
    {
        //рандомайзер
        Random rng = new Random();

        // игровое поле 
        int[,] field;

        // максимальное число мин
        int maxMines;

        // установка значений 
        public FieldGen(int difficulty)
        {
            if (difficulty == 0)
            {
                // заполнение поля
                field = new int[4, 4];

                // установка кол-ва мин
                maxMines = 3;
            }
            if (difficulty == 1)
            {
                // заполнение поля
                field = new int[7, 7];

                // установка кол-ва мин
                maxMines = 12;
            }
            if (difficulty == 2)
            {
                // заполнение поля
                field = new int[11, 11];

                // установка кол-ва мин
                maxMines = 30;
            }
        }

        public FieldGen(int difficulty, int N, int M)
        {
            if (difficulty == 0)
            {
                // установка кол-ва мин
                maxMines = (int)(N * M * 20 / 100);
            }
            if (difficulty == 1)
            {
                // установка кол-ва мин
                maxMines = (int)(N * M * 25 / 100);
            }
            if (difficulty == 2)
            {
                // установка кол-ва мин
                maxMines = (int)(N * M * 30 / 100);
            }

            field = new int[N, M];
        }

        // проверка на наличике мин вокруг ячейки 
        bool ChekMinesEnv()
        {
            bool res = false;

            // пробежка по клеткам вокруг
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    // встреча с миной -> смерть генерации мин 
                    if (field[i, j] == 9)
                    {
                        int l = i - 1;
                        if (l < 0) l = 0;

                        int r = i + 1;
                        if (r >= field.GetLength(0)) r = field.GetLength(0) - 1;

                        int u = j - 1;
                        if (u < 0) u = 0;

                        int d = j + 1;
                        if (d >= field.GetLength(1)) d = field.GetLength(1) - 1;

                        // обход соседей вокруг 
                        for (int i1 = l; i1 <= r; i1++)
                            for (int j1 = u; j1 <= d; j1++)
                                if (field[i1, j1] == 0)
                                {
                                    // встреча с пустой клеткой -> не смерть 
                                    res = true;

                                    // выходим и говорим, что можно оставить мину 
                                    break;
                                }

                        // если несмерть не наступила перестаём генерить мину 
                        if (res == false) return false;
                    }  
                }

            // можно оставить мину, всё ок, ничего не ломается 
            return res; 
        }

        //установка мин 
        void plantMines()
        {
            for (int i = 0; i < maxMines;)
            {
                // рандоминг координат 
                int x = rng.Next(0, field.GetLength(0));
                int y = rng.Next(0, field.GetLength(1));

                // проверка на наличие мины в координатах
                if (field[x, y] == 9)
                    continue;

                // установка мины 
                field[x, y] = 9;

                // если мина ломает игру
                if (ChekMinesEnv() == false)
                {
                    // обнуляем 
                    field[x, y] = 0;
                    continue;
                }

                i++;
            }
        }

        // вычисление цифр в ячейках 
        void calculateCells()
        {
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == 0)
                    {
                        int l = i - 1;
                        if (l < 0) l = 0;

                        int r = i + 1;
                        if (r >= field.GetLength(0)) r = field.GetLength(0) - 1;

                        int u = j - 1;
                        if (u < 0) u = 0;

                        int d = j + 1;
                        if (d >= field.GetLength(1)) d = field.GetLength(1) - 1;

                        int sum = 0; 

                        // обход соседей вокруг и подсчёт мин  
                        for (int i1 = l; i1 <= r; i1++)
                            for (int j1 = u; j1 <= d; j1++)
                                if (field[i1, j1] == 9)
                                    sum++;

                        field[i, j] = sum;
                    }
                }
        }

        // генерация 
        public void generate()
        {
            plantMines();
            calculateCells();
        }

        public int getCell(int i, int j)
        {
            return field[i, j];
        }

    }
}
