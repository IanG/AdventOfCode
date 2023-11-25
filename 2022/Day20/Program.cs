List<int> inputs = System.IO.File.ReadLines(args[0]).Select(Int32.Parse).ToList();
List<int> decrypted = new List<int>(inputs);

Console.WriteLine("Wait");

int inputPosition = 0;
while (true)
{
    int next = inputs[inputPosition];
    int currentPosition = decrypted.IndexOf(next);

    inputPosition++;
}
