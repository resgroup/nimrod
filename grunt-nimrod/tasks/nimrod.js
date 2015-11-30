var child = require("child_process");
module.exports = function (grunt) {
    grunt.registerMultiTask('nimrod', 'An ASP.NET MVC to TypeScript Converter', function () {
        var task = this;
        var done = task.async();
        var options = task.options({});
        var verbose = '';
        if (options.verbose === true) {
            verbose = ' --verbose';
        }
        var cmd = __dirname + '\\Nimrod.Console\\bin\\Release\\Nimrod.Console.exe -m ' + options.module + ' -o ' + options.output + ' --files=' + options.files.join(':') + verbose;
        var childProcess = child.exec(cmd);
        childProcess.stdout.on('data', function (data) {
            grunt.log.write(data);
        });
        childProcess.stderr.on('data', function (data) {
            grunt.log.error(data);
        });
        childProcess.on('error', function (error) {
            grunt.log.error('Failed with: ' + error);
            done(false);
        });
        childProcess.on('exit', function (code) {
            grunt.log.write('Exited with code: ' + code);
            done();
        });
    });
};
//# sourceMappingURL=nimrod.js.map