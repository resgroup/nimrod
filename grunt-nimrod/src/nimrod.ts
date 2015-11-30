/*
 * grunt-nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 RES
 * Licensed under the MIT license.
 */
import * as child from "child_process";

export interface IGruntNimrodOptions {
    verbose?: boolean;
    module?: string;
    output?: string;
    files?: string[];
}

module.exports = function (grunt: IGrunt) {
    grunt.registerMultiTask('nimrod', 'An ASP.NET MVC to TypeScript Converter', function () {

        var task = <grunt.task.ITask>this; 
        var done = task.async();

        var options = task.options<IGruntNimrodOptions>({});

        var verbose = '';
        if (options.verbose === true) {
            verbose = ' --verbose';
        }
        
        var cmd = __dirname + '\\Nimrod.Console\\bin\\Release\\Nimrod.Console.exe -m ' + options.module + ' -o ' + options.output + ' --files=' + options.files.join(':') + verbose;

        var childProcess = child.exec(cmd);

        childProcess.stdout.on('data', (data: any) => {
            grunt.log.write(data);
        });

        childProcess.stderr.on('data', (data: any) => {
            grunt.log.error(data);
        });

        childProcess.on('error', (error: any) => {
            grunt.log.error('Failed with: ' + error);
            done(false);
        });

        childProcess.on('exit', (code: any) => {
            grunt.log.write('Exited with code: ' + code);
            done();
        });
    });
};
