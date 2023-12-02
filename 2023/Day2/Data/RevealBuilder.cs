namespace Day2.Data;

public class RevealBuilder
{
    private const char CubeDelimiter = ',';
    
    private readonly string _data;
    
    public RevealBuilder(string data)
    {
        _data = data;
    }

    public Reveal Build()
    {
        int redCubes = 0;
        int greenCubes = 0;
        int blueCubes = 0;
        
        foreach (string cube in _data.Split(CubeDelimiter))
        {
            string cubeData = cube.Trim();
            int spacePos = cubeData.IndexOf(' ');
            int cubeCount = int.Parse(cubeData[..spacePos]);
            
            switch(cubeData[(spacePos + 1)..])
            {
                case "red": redCubes = cubeCount;
                    break;
                case "green": greenCubes = cubeCount;
                    break;
                case "blue": blueCubes = cubeCount;
                    break;
            }
        }
        
        return new Reveal { RedCubes = redCubes, GreenCubes = greenCubes, BlueCubes = blueCubes };
    }
}