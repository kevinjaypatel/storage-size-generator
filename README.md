# Lazy Evaluation in C#  

This program uses generators in C# for iterating through a sequence of files. By using lazy evaluation, the programmer is able to preserve memory and modularize code by separating logic in different functions. This program takes two inputs from the command line respectively: 

* path of a directory 
* name of an output file 

In effect, the program outputs an HTML table within the project directory. The output file renders a list of grouped file extensions, their total count, and their total size in bytes.  


## Installation

1. Clone the repository 
 


## Usage
1. In Visual Studio, select Tools -> Command Line -> Developer Command Prompt
2. Compile the program: csc GeneratorProgram.cs
3. After successful compilation, run the program like so: GeneratorProgram "[path-of-testDirectory]" "output.html" 

## Contributing
Pull requests are welcome. 

## License
[MIT](https://choosealicense.com/licenses/mit/)
