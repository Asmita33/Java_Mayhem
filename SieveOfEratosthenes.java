package Codes;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Arrays;
import java.util.Scanner;

public class SieveOfEratosthenes
{
    public static void sieveOfEratosthenes(int n)
    {
        // 1) create a new file for this class and create a null fileWriter
        String fileName = "simpleArray.txt";
        File myFile = new File("F:\\stateOfVariables.txt");

        FileWriter fw = null;


        try {

            boolean[] prime = new boolean[n + 1];
            for (int i = 0; i <= n; i++)
                prime[i] = true;

            // 2) whenever writing the next state to a file:
            // 2.a) first clear the previous contents/state of the file
            fw = new FileWriter(myFile);

            // 2.b) write new content/state to the file
            fw.write("prime = " +Arrays.toString(prime) +"\n");
            fw.close();

            int p = 2;
            for (p = 2; p * p <= n; p++) {
                if (prime[p]) {


                    for (int i = p * p; i <= n; i += p) {

                        // i) first do the work
                        prime[i] = false;

                        // ii) then write to file
                        fw = new FileWriter(myFile);
                        fw.write("p = " +p +"\n");
                        fw.write("i = " +i +"\n");
                        fw.write("prime = " +Arrays.toString(prime) +"\n");
                        fw.close();

                    }
                }
            }

            for (int i = 2; i <= n; i++) {
                if (prime[i])
                    System.out.print(i + " ");
            }

            fw.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static void main(String[] args)
    {
        int n = 10;
        sieveOfEratosthenes(n);
    }
}
