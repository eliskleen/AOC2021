int [,] foldy(int [,] paper, int foldY)
{
    var xMax = paper.GetLength(0);
    var yMax = paper.GetLength(1);
    var ret = new int [xMax, foldY];
    for (int x = 0; x < xMax; x++)
    {
        for (int y = 0; y < yMax; y++)
        {
            if(y == foldY)
                continue;
            if(y > foldY) 
            {
                var ydiff = y-foldY;
                ret[x, foldY-ydiff] += paper[x, y];
            }
            else
                ret[x, y] += paper[x, y];
        }
    }
    return ret;
}
int [,] foldx(int [,] paper, int foldX)
{
    var xMax = paper.GetLength(0);
    var yMax = paper.GetLength(1);
    var ret = new int [foldX, yMax];
    for (int x = 0; x < xMax; x++)
    {
        for (int y = 0; y < yMax; y++)
        {
            if(x == foldX)
                continue;
            if(x > foldX)
            {   
                var xDiff = x-foldX; 
                ret[foldX-xDiff, y] += paper[x, y];
            } 
            else
                ret[x, y] += paper[x, y];
        }
    }
    return ret;
}
int [,] star1(int [,] paper, (string, int) fold)
{
    if(fold.Item1.Equals("x"))
        return foldx(paper, fold.Item2);
    
    return foldy(paper, fold.Item2);
}
void star2(int [,] paper, List<(string, int)> folds, bool print = true)
{
    foreach (var f in folds)
    {
        if(f.Item1.Equals("x"))
            paper = foldx(paper, f.Item2);
        else
            paper = foldy(paper, f.Item2);
    }

    if(!print)
        return;
    for (int x = 0; x < paper.GetLength(1); x++)
    {
        for (int y = 0; y < paper.GetLength(0); y++)
        {
            Console.Write(paper[y, x] > 0 ? "#" : " ");
        }
        Console.WriteLine("");
    }
}
long main()
{
    var lines = File.ReadAllLines("input.txt");
    var watch = new Stopwatch();
    watch.Start();
    var coords = new List<(int, int)>();
    var folds = new List<(string, int)>();
    int xMax = 0;
    int yMax = 0;
    foreach (var line in lines)
    {
        if(line.Contains(','))
        {
            var c = line.Split(',').ToList().ConvertAll(int.Parse);
            coords.Add((c[0], c[1]));
            xMax = xMax < c[0] ? c[0] : xMax;
            yMax = yMax < c[01] ? c[1] : yMax;
            continue;
        }
        if(line.Contains('='))
        {
            var f = line.Split(' ').Last();
            var s = f.Split('=');
            folds.Add((s[0], int.Parse(s[1])));
        }
    }
    var paper =  new int[xMax + 1, yMax + 1];
    foreach (var c in coords)
        paper[c.Item1, c.Item2] = 1;
    var folded = star1(paper, folds[0]);
    //Console.WriteLine(folded.Cast<int>().ToList().Count(s => s > 0));
    star2(paper, folds);
    watch.Stop();
    return watch.ElapsedMilliseconds;
}
long sum = 0;
var times = 1;
for(int i = 0; i< times; i++)
    sum += main();
Console.WriteLine("Avarage of "+ times+ " runs: "+(float)sum/(float)times);
Console.WriteLine(sum);