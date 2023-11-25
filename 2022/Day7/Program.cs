IEnumerable<string> input = System.IO.File.ReadLines(args[0]);

const string COMMAND_PROMPT = "$";
const string PARENT_DIRECTORY = "..";
const string CHANGE_DIRECTORY_COMMAND = "cd";
const string LIST_DIRECTORY_COMMAND = "ls";
const string DIR = "dir";

Stack<string> path = new Stack<string>();

foreach (string line in input)
{
    string[] arguments = line.Split(' ');

    if (COMMAND_PROMPT.Equals(arguments[0])) 
    {
        Console.WriteLine($"Command: {String.Join(" ", arguments[Range.StartAt(1)])}");
        switch(arguments[1])
        {
            case CHANGE_DIRECTORY_COMMAND:
                if (PARENT_DIRECTORY.Equals(arguments[2]))
                {
                    if (path.Count > 0) path.Pop();
                }
                else
                {
                    string newPath = arguments[2];
                    if (path.Count > 0) newPath += '/';
                    path.Push(newPath);
                }
                Console.WriteLine($"Path: {String.Join("", path.AsEnumerable().Reverse())}");
                break;
            
            case LIST_DIRECTORY_COMMAND:
                break;
        }
    }
    else
    {
        if(!arguments[0].StartsWith(DIR))
        {
            Console.WriteLine($"File {arguments[1]} Size {arguments[0]}");
        }
        else
        {
            Console.WriteLine($"Directory {arguments[1]}");
        }
        
    }
}

Console.WriteLine("Wait");

void ProcessCommand(string[] arguments)
{
    Console.WriteLine($"Command: {String.Join(" ", arguments)}");
    switch(arguments[0])
    {
        case CHANGE_DIRECTORY_COMMAND:
            if (PARENT_DIRECTORY.Equals(arguments[1]))
            {
                if (path.Count > 0) path.Pop();
            }
            else
            {
                string newPath = arguments[1];
                if (path.Count > 0) newPath += '/';
                path.Push(newPath);
            }
            break;
        
        case LIST_DIRECTORY_COMMAND:
            break;
    }
    Console.WriteLine($"Path: {String.Join("", path.AsEnumerable().Reverse())}");
}
