# Lazy Evaluation in C#  

This program uses generators in C# for iterating through a sequence of files. By using lazy evaluation, the programmer is able to preserve memory and modularize code by separating logic in different functions. This program takes two inputs from the command line respectively: 

* path of a directory 
* name of an output file 

The program creates an HTML file within the project directory. The output file renders an HTML table that includes a list of grouped file extensions, their total count, and their total size in bytes.  

## Installation

1. Clone the repository 
 


## Usage
1. In Visual Studio, select Tools -> Command Line -> Developer Command Prompt
2. Compile the program by running the following command in the developer command prompt: csc GeneratorProgram.cs
3. After successful compilation, run the program using the following command where 'output.html' will be the name of the file created by the program: GeneratorProgram "[path-of-testDirectory]" "output.html" 

## Contributing
Pull requests are welcome. 

## License
[MIT](https://choosealicense.com/licenses/mit/)
