/*
 * grunt-nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 RES
 * Licensed under the MIT license.
 */
import * as child from 'child_process';

export interface IGruntNimrodOptions {
    exe?: string;
    verbose?: boolean;
    output?: string;
    strictNullCheck?: boolean;
    files?: string[];
}

module.exports = function (grunt: IGrunt) {
    grunt.registerMultiTask('nimrod', 'An ASP.NET MVC to TypeScript Converter', function () {

        let task = this as grunt.task.ITask;
        let done = task.async();

        let options = task.options<IGruntNimrodOptions>({});

        let verbose = '';
        if (options.verbose === true) {
            verbose = ' --verbose';
        }

        let strictNullCheck = '';
        if (options.strictNullCheck === true) {
            strictNullCheck = ' --strictNullCheck';
        }

        let pathExe = options.exe || __dirname + '\\Nimrod.Console\\bin\\Release\\Nimrod.Console.exe';

        let cmd = pathExe + ' -o ' + options.output + ' --files=' + options.files.join(',') + verbose + strictNullCheck;
        if (options.verbose) {
            grunt.log.write('Executing command : ' + cmd);
        }
        let childProcess = child.exec(cmd);

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
