const int START_OF_PACKET_MARKER_LENGTH = 4;
const int START_OF_MESSAGE_MARKER_LENGTH = 14;

char[] input = System.IO.File.ReadLines(args[0]).First().ToCharArray();

Console.WriteLine($"Part 1: {GetFirstMarkerPosition(START_OF_PACKET_MARKER_LENGTH, input)}");
Console.WriteLine($"Part 2: {GetFirstMarkerPosition(START_OF_MESSAGE_MARKER_LENGTH, input)}");

int GetFirstMarkerPosition(int markerLength, char[] buffer)
{
    List<char> marker = new List<char>();

    for (int c = 0; c < buffer.Length; c++)
    {
        if (marker.Count == markerLength)
        {
            if (marker.Distinct().Count() != markerLength)
            {
                marker.RemoveAt(0);
                marker.Add(buffer[c]);
            }
            else
            {
                return c;
            }
        }
        else
            marker.Add(buffer[c]);
    }
    return 0;
}