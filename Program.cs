using System;
using System.IO;
using System.Linq;

namespace bsthsmc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string programLocation = AppContext.BaseDirectory;
            string destination = "";
            bool noTree = false;
            string soundset = "";
            byte soundsetIdBase = 0x0F;

            if(args.Length == 0)
            {
                Console.WriteLine("Usage:\n" +
                                  "\tbsthsmc.exe <soundset> <destination> <sound(s)>\n\n" +
                                  "\t<soundset> - which hitsound set should be replaced. Can be 0, 1, 2 or \"default\", \"clap\", \"tambourine\" respectively.\n" +
                                  "\t<destination> - name of the folder where new hitsounds will be placed. If not specified and you passed in multiple sounds, you will be prompted to specify the location.\n" +
                                  "\t<sound(-s)> - properly named Microsoft ADPCM formatted WAV file. It's possible to pass in multiple sounds at once, just separate filenames by spaces.\n" +
                                  "\t\t- tap.wav/basic.wav/note.wav\n" +
                                  "\t\t- slash.wav/flick.wav\n" +
                                  "\t\t- ripple.wav\n" +
                                  "\t\t- stream.wav\n" +
                                  "\n" +
                                  "\tAlternatively, you can just drag and drop your sounds onto bsthsmc.exe.\n" +
                                  "\tIf destination is not specified/empty, files will be created in the same directory as bsthsmc.exe without proper directory tree.\n" +
                                  "\tIf name of the sound doesn't match templates, you will be prompted to select hitsound type." +
                                  "\tIf soundset is not specified, you will be prompted to choose a soundset to replace.");
                Console.ReadKey();
            }
            else
            {

                foreach (string arg in args)
                {
                    if (arg.EndsWith(".wav"))
                    {
                        if (soundset.Trim().Length == 0)
                        {
                            Console.WriteLine("\nPlease choose a soundset to replace (0 - default, 1 - clap, 2 - tambourine)");
                            switch (Console.ReadLine())
                            {
                                case "0":
                                    soundset = "default1";
                                    break;
                                case "1":
                                    soundset = "clap";
                                    soundsetIdBase = 0x13;
                                    break;
                                case "2":
                                    soundset = "tambourine";
                                    soundsetIdBase = 0x17;
                                    break;
                                default:
                                    Console.WriteLine("Incorrect soundset, using \"defauld\" set");
                                    break;
                            }
                        }

                        byte[] headerData = new byte[]
                        {
                            0x53, 0x44, 0x39, 0x00,     //0 - Header
                            0x20, 0x00, 0x00, 0x00,     //4 - Header size
                            0xFF, 0xFF, 0xFF, 0xFF,     //8 - Byte length of sound. Should be set in code.
                            0x31, 0x32, 0x04, 0x00,     //12 - Something. Byte 14 should be set in code.
                            0x40, 0x00, 0x01, 0x00,     //16 - Something.
                            0x00, 0x00, 0x00, 0x00,     //20 - Something.
                            0x00, 0x00, 0x00, 0x00,     //24 - Something.
                            0x00, 0x00, 0xFF, 0x00,     //28 - Something. 30 - 2 byte ID, should be set in code.
                        };
                        byte[] soundData = File.ReadAllBytes(arg);
                        byte[] finalData;
                        byte soundType = 0;

                        BitConverter.GetBytes(soundData.Length).CopyTo(headerData, 8);

                        switch (arg.ToLower())
                        {
                            case "tap.wav":
                                headerData[30] = soundsetIdBase;
                                soundType = 0;
                                break;
                            case "basic.wav":
                                headerData[30] = soundsetIdBase;
                                soundType = 0;
                                break;
                            case "normal.wav":
                                headerData[30] = soundsetIdBase;
                                soundType = 0;
                                break;
                            case "note.wav":
                                headerData[30] = soundsetIdBase;
                                soundType = 0;
                                break;
                            case "slash.wav":
                                headerData[30] = (byte)(soundsetIdBase + 1);
                                soundType = 1;
                                break;
                            case "flick.wav":
                                headerData[30] = (byte)(soundsetIdBase + 1);
                                soundType = 1;
                                break;
                            case "ripple.wav":
                                headerData[30] = (byte)(soundsetIdBase + 2);
                                soundType = 2;
                                break;
                            case "stream.wav":
                                headerData[14] = 0x05;
                                headerData[30] = (byte)(soundsetIdBase + 3);
                                soundType = 3;
                                break;
                            default:
                                Console.WriteLine($"\nPlease select a note type for {arg} (0 - tap, 1 - slash, 2 - ripple, 3 - stream)");
                                switch (Console.ReadLine())
                                {
                                    case "0":
                                        headerData[30] = soundsetIdBase;
                                        soundType = 0;
                                        break;
                                    case "1":
                                        headerData[30] = (byte)(soundsetIdBase + 1);
                                        soundType = 1;
                                        break;
                                    case "2":
                                        headerData[30] = (byte)(soundsetIdBase + 2);
                                        soundType = 2;
                                        break;
                                    case "3":
                                        headerData[14] = 0x05;
                                        headerData[30] = (byte)(soundsetIdBase + 3);
                                        soundType = 3;
                                        break;
                                    default:
                                        Console.WriteLine("Incorrect type, aborting....");
                                        Console.ReadKey();
                                        return;
                                }
                                break;
                        }
                        finalData = headerData.Concat(soundData).ToArray();

                        if (destination.Trim().Length == 0)
                        {
                            Console.WriteLine("Please specify a destination folder:");
                            string path = Console.ReadLine();
                            if (path.Trim().Length == 0)
                            {
                                destination = programLocation;
                                noTree = true;
                            }
                            else destination = path;
                        }

                        if (!(destination.EndsWith('\\') || destination.EndsWith('/'))) destination += '\\';

                        if (!noTree)
                        {
                            Directory.CreateDirectory(destination + "sound\\system\\");
                            Directory.CreateDirectory(destination + "data2\\sound\\system\\");

                            switch (soundType)
                            {
                                case 0:
                                    File.WriteAllBytes(destination + $"sound\\system\\sys_se_shot_{soundset}_basic.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"data2\\sound\\system\\bs1_shot_{soundset}_basic.sd9", finalData);
                                    break;
                                case 1:
                                    File.WriteAllBytes(destination + $"sound\\system\\sys_se_shot_{soundset}_flick.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"data2\\sound\\system\\bs1_shot_{soundset}_flick.sd9", finalData);
                                    break;
                                case 2:
                                    File.WriteAllBytes(destination + $"sound\\system\\sys_se_shot_{soundset}_ripple.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"data2\\sound\\system\\bs1_shot_{soundset}_ripple.sd9", finalData);
                                    break;
                                case 3:
                                    File.WriteAllBytes(destination + $"sound\\system\\sys_se_shot_{soundset}_stream.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"data2\\sound\\system\\bs1_shot_{soundset}_stream.sd9", finalData);
                                    break;
                            }
                        }
                        else
                            switch (soundType)
                            {
                                case 0:
                                    File.WriteAllBytes(destination + $"sys_se_shot_{soundset}_basic.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"bs1_shot_{soundset}_basic.sd9", finalData);
                                    break;
                                case 1:
                                    File.WriteAllBytes(destination + $"sys_se_shot_{soundset}_flick.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"bs1_shot_{soundset}_flick.sd9", finalData);
                                    break;
                                case 2:
                                    File.WriteAllBytes(destination + $"sys_se_shot_{soundset}_ripple.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"bs1_shot_{soundset}_ripple.sd9", finalData);
                                    break;
                                case 3:
                                    File.WriteAllBytes(destination + $"sys_se_shot_{soundset}_stream.sd9", finalData);
                                    finalData[30]++;
                                    File.WriteAllBytes(destination + $"bs1_shot_{soundset}_stream.sd9", finalData);
                                    break;
                            }
                    }
                    else
                        switch (arg)
                        {
                            case "0":
                                soundset = "default1";
                                break;
                            case "1":
                                soundset = "clap";
                                break;
                            case "2":
                                soundset = "tambourine";
                                break;
                            case "default":
                                soundset = "default1";
                                break;
                            case "clap":
                                soundset = "clap";
                                break;
                            case "tambourine":
                                soundset = "tambourine";
                                break;
                            default:
                                destination = arg;
                                break;
                        }
                }
            }
        }
    }
}
