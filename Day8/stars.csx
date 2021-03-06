 var segCodes = new string[10] {"abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg",
                                  "acf", "abcdefg", "abcdfg"};
long main()
{
    var watch = new Stopwatch();
    var lines = File.ReadAllLines("input.txt").ToList();
    watch.Start();
   
    star1(segCodes, lines); //~0,48 ms on avarage when run many times in a row, 5 when ran once
    //star2(segCodes.ToList(), lines); //3,15, 16 when ran once
    star2Improved(lines); //~on avarage 2,57ms to run when run many times in a row but around 10 when ran once
    watch.Stop();
    Console.WriteLine(watch.ElapsedMilliseconds); //~on avarage 3 ms to solve both when run many times in a row, 15-20 on first run
    return watch.ElapsedMilliseconds;
}
void star1(string [] segCodes, List<string> lines)
{
    var numbers = new int[4] {1,4,7,8}.ToList();
    var usedCodes = numbers.Select(s => segCodes[s]).ToList();
    long sum = 0;
    foreach (var line in lines)
    {
        var result = line.Split('|')[1].Split(' ').ToList();
        result = result.Where(s => usedCodes.Any(c => c.Length == s.Length)).ToList();
        sum += result.Count;
    }
    Console.WriteLine(sum);
}
void star2Improved(List<string> lines)
{
    var sum = 0;
    segCodes.ToList().ForEach(s => s = String.Concat(s.OrderBy(c => c))); 
     foreach (var line in lines)
    {
        var input = line.Split('|')[0].Split(' ').ToList()
                        .Where(s => s.Length == 2 
                                 || s.Length == 3
                                 || s.Length == 4
                                 || s.Length == 7).ToList();
        var result = line.Split('|')[1].Split(' ').ToList();
        result.Remove("");
        var numbers = result.Select(r => calcNum2(r, input)).ToList();
        numbers.Reverse();
        //count coudes:
        for (int i = 0; i < result.Count; i++)
            sum += numbers[i]*(int)Math.Pow(10, i);   
    }
    Console.WriteLine(sum);
}

int calcNum2(string code, List<string> input)
{
    var len = code.Length;
    if(len == 2)
        return 1;
    if(len == 3)
        return 7;
    if(len == 4)
        return 4;
    if(len == 7)
        return 8;
    var ls = code.ToList(); 
    var a = ls.RemoveAll(s => Convert.ToBoolean(input.FirstOrDefault(s => s.Length == 2).Contains(s)));
    var b = ls.RemoveAll(s => Convert.ToBoolean(input.FirstOrDefault(s => s.Length == 4).Contains(s)));
    var c = ls.RemoveAll(s => Convert.ToBoolean(input.FirstOrDefault(s => s.Length == 3).Contains(s)));
    var d = ls.RemoveAll(s => Convert.ToBoolean(input.FirstOrDefault(s => s.Length == 7)?.Contains(s)));
     if(a == 2 && d == 2)
        return 0;
    if(d == 2 && b == 1)
        return 2;
    if(a == 2 && b == 1)
        return 3;
    if(a == 1 && b == 2 && d == 1)
        return 5; 
    if(a == 1)
        return 6;
    return 9; 

}
void star2(List<string> segCodes, List<string> lines)
{
    var sum = 0;
    segCodes.ForEach(s => s = String.Concat(s.OrderBy(c => c)));
    var usedCodes = segCodes;
    foreach (var line in lines)
    {
        var input = line.Split('|')[0].Split(' ').ToList();
        var result = line.Split('|')[1].Split(' ').ToList();
        result.Remove("");
        input.Remove("");
        var replacers = getReplacers(segCodes, input);
        replacers = replacers.Distinct().ToList();
        result = result.Select(i => replaceAll(i, replacers)).ToList();
        var numbers = result.Select(r => calcNum(r)).ToList();
        numbers.Reverse();
        //count coudes:
        for (int i = 0; i < result.Count; i++)
            sum += numbers[i]*(int)Math.Pow(10, i);   
    }
    Console.WriteLine(sum);
}
string replaceAll(string str, List<(char, char)> replacers)
{
    foreach (var rep in replacers)
        str = str.Replace(rep.Item1, rep.Item2);
    return str;
}
int calcNum(string code)
{
    if(code.Length == 2)
        return 1;
    if(code.Length == 3)
        return 7;
    if(code.Length == 4)
        return 4;
    if(code.Length == 7)
        return 8;

    var ls = code.ToList(); 
    var a = ls.Count(s => s == '1');
    var b = ls.Count(s => s == '2');
    var c = ls.Count(s => s == '3');
    var d = ls.Count(s => s == '4');
    if(a == 2 && d == 2)
        return 0;
    if(d == 2 && b == 1)
        return 2;
    if(a == 2 && b == 1)
        return 3;
    if(a == 1 && b == 2 && d == 1)
        return 5; 
    if(a == 1)
        return 6;
    
    return 9;


}
List<(char, char)> getReplacers(List<string> segCodes, List<string> input)
{
    var replacers = new List<(char, char)>();
    addReplacer(segCodes[1], replacers, input, '1');
    addReplacer(segCodes[4], replacers, input, '2');
    addReplacer(segCodes[7], replacers, input, '3');
    addReplacer(segCodes[8], replacers, input, '4');
    return replacers;

}
void addReplacer(string segCode, List<(char, char)> replacers, List<string> input, char rep)
{
    var current = input.FirstOrDefault(s => s.Length == segCode.Length).ToList();
    replacers.ForEach(r => current.RemoveAll(s => s == r.Item1));
    current.RemoveAll(s => replacers.Select(r => r.Item1).Contains(s)
                                 || replacers.Select(r => r.Item2).Contains(s));
    foreach (var c in current)
        replacers.Add((c, rep));
}

long sum = 0;
var times = 1;
for(int i = 0; i< times; i++)
    sum += main();
Console.WriteLine("Avarage of "+times+ " runs: "+(float)sum/(float)times);
Console.WriteLine(sum);