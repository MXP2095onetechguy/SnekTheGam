using System;
using System.IO;


namespace BinIOUtils{

    class BinIO{
        int WriteBin(string fileName, string text){
            try{
                using (BinaryWriter binWriter = new BinaryWriter(File.Open(fileName, FileMode.Create)))  
                {  
                    // Write string   
                    binWriter.Write(text);
                } 
            }
            catch(IOException){
                return 1;
            }

            return 0;
        }

        string ReadBin(string fileName){
            string text = "";

            try{
                using(BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open)){
                    text = binReader.ReadString();
                }
            }
            catch(IOException e){
                return;
            }

            return text;
        }
    }


}