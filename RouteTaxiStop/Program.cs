using System;

namespace RouteTaxiStop
{
   class Program
    {
        public static void Main(string[] args)
        {
            BusStop b = new BusStop();
            b.CreateBus(20);
            b.mainLoop();
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }
        static class Randomizer
{
    private static readonly Random randomizer = new Random();
 
    public static int Next(int max)
        => randomizer.Next(max);
}
 
class People
{
    public int tCome; // время когда пришел на остановку
    public People next;
 
    public People()
    {
        tCome = 0;
    }
 
    public People(int t)
    {
        tCome = t;
    }
 
    public bool add(int t)
    {
        if (next != null)
        {
            next.add(t);
        }
        else
        {
            next = new People(t);
        }
        return true;
    }
}
 
class Peoples
{
    public People root;
    public int countPeople;
    public int countBigData; // количество народу , которые уехали
    public int waitBigData; // Общее время ожидания в минутах
 
    public Peoples()
    {
        countPeople = 0;
        waitBigData = 0;
        countBigData = 0;
    }
 
    public bool remove(int t)
    {
        if (root == null)
        {
            return false;
        }
 
        People tmp = root.next;
        countBigData++;
        countPeople--;
        waitBigData = waitBigData + (t - root.tCome);
        Console.Write($"{t - root.tCome} ({waitBigData} ) , ");
        root = tmp;
        return true;
    }
 
    public bool away(int t, int c)
    {
        Console.Write("\n\t + Уехали:  ");
        for (int i = 0; i < c; i++)
        {
            remove(t);
        }
        return true;
    }
 
    public bool add(int t)
    {
        if (root != null)
        {
            root.add(t);
        }
        else
        {
            root = new People(t);
        }
        countPeople++;
        return countPeople != 0;
    }
 
    public bool comeToBusStop(int t, int dt)
    {
        int k = getIntervalByPartDay(t);
        Console.Write($"\n\t ! на остановку пришло {k}");
        for (int i = 0; i < k; i++)
        {
            add(t);
        }
        return true;
    }
 
    public int getIntervalByPartDay(int t) // количество пассажиров , которые пришли на остановку в минуту
    {
        int min, max;
        PART_OF_DAY vs;
 
        if (0 < t && t < 420)
        {
            vs = PART_OF_DAY.Night;
        }
        else if (420 < t && t < 720)
        {
            vs = PART_OF_DAY.Morning;
        }
        else if (720 < t && t < 1020)
        {
            vs = PART_OF_DAY.Day;
        }
        else
        {
            vs = PART_OF_DAY.Evening;
        }
 
        switch (vs)
        {
            case PART_OF_DAY.Night:
                min = 0; max = 1;
                break;
            case PART_OF_DAY.Morning:
                min = 1; max = 5;
                break;
            case PART_OF_DAY.Day:
                min = 2; max = 5;
                break;
            case PART_OF_DAY.Evening:
                min = 3; max = 5;
                break;
            default:
                min = 0; max = 0;
                break;
        }
        return Randomizer.Next(max) + min;
    }
}
 
class Bus
{
    public int[] interval = new int[4]; // Храним интервалы в зависимости от времени суток
    public int waitTime; // Сколько минут прошло с появлением последней маршрутки
    public int max; // Сколько максимальное количество мест в маршрутке
    public int number;
 
    public Bus()
    {
        waitTime = 5;
    }
 
    public bool changeData(int num, int[] interval)
    {
        if (interval.Length != 4)
        {
            throw new ArgumentException(null, nameof(interval));
        }
 
        for (int i = 0; i < 4; i++)
        {
            interval[i] = interval[i];
        }
        number = num;
        max = Randomizer.Next(10) + 10;
        return true;
    }
 
    public int getIntervalByPartDay(int t)
    {
        int interval;
 
        PART_OF_DAY vs;
 
        if (0 < t && t < 420)
        {
            vs = PART_OF_DAY.Night;
        }
        else if (420 < t && t < 720)
        {
            vs = PART_OF_DAY.Morning;
        }
        else if (720 < t && t < 1020)
        {
            vs = PART_OF_DAY.Day;
        }
        else
        {
            vs = PART_OF_DAY.Evening;
        }
 
        switch (vs)
        {
            case PART_OF_DAY.Night:
                interval = this.interval[(int)PART_OF_DAY.Night];
                break;
            case PART_OF_DAY.Morning:
                interval = this.interval[(int)PART_OF_DAY.Morning];
                break;
            case PART_OF_DAY.Day:
                interval = this.interval[(int)PART_OF_DAY.Day];
                break;
            case PART_OF_DAY.Evening:
                interval = this.interval[(int)PART_OF_DAY.Evening];
                break;
            default:
                interval = 0;
                break;
        }
        return interval;
    }
    public int getNumber()
    {
        return number;
    }
    public int comeToBusStop(int t, int dt) // Возвращает количество мест в маршрутке 
                                            // или 0 если маршрутка еще не пришла
    {
        waitTime = waitTime - dt;
        if (waitTime > 0)
        {
            return 0;
        }
        waitTime = getIntervalByPartDay(t);
        return Randomizer.Next(max) + 1;
    }
}
 
class BusStop
{
    public Bus[] bus;
    public int countBus;
    public Peoples peoples;
 
    public BusStop()
    {
        peoples = new Peoples();
    }
 
    public bool CreateBus(int count)
    {
        var @in = new int[4];
        bus = new Bus[count];
        for (int i = 0; i < count; i++)
        {
            bus[i] = new Bus();
            @in[0] = Randomizer.Next(30) + 30;
            @in[1] = Randomizer.Next(10) + 10;
            @in[2] = Randomizer.Next(10) + 15;
            @in[3] = Randomizer.Next(10) + 10;
        }
        countBus = count;
        return true;
    }
 
    public bool mainLoop()
    {
        int mest;
        for (int t = 0; t < 60 * 24; t++)
        {
            Console.Write(".");
            // пришли люди
            peoples.comeToBusStop(t, 1);
 
            // Приехал автобус
            for (int b = 0; b < countBus; b++)
            {
                mest = bus[b].comeToBusStop(t, 1);
                if (mest > 0)
                {
                    Console.Write($"\n--> Автобус № {bus[b].getNumber()} Пустых мест {mest} На остановке {peoples.countPeople}");
                    peoples.away(t, mest);
                    Console.Write($"/ Осталось {peoples.countPeople}");
                }
            }
        }
 
        Console.Write("\n\n_____________________Статистика__________________");
        Console.Write($"\n Перевезено пассажиров: {peoples.countBigData}");
        Console.Write($"\n Люди потеряли минут  {peoples.waitBigData}");
        Console.Write($"\n Среднее время ожидания : {(double)peoples.waitBigData / peoples.countBigData}");
        return true;
    }
}
 
enum PART_OF_DAY { Night = 0, Morning, Day, Evening };
        }
    }
