/// <binding />

var customScripts = [
    'src/**/*.d.ts',
    'src/nimrod.demo.module.ts',
    'src/**/*.enum.ts',
    'src/**/*.class.ts',
    'src/ServerApi.Generated/**/*.ts',
    'src/**/*.service.ts',
    'src/**/*.directive.ts',
    'src/**/*.component.ts',
    'src/**/*.filter.ts',
    'src/**/*.controller.ts'
];

module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        nimrod: {
            default_options: {
                options: {
                    // specify the dev executable for the demo
                    exe: '..\\..\\Nimrod.Console\\bin\\Release\\Nimrod.Console.exe',
                    module: 'typescript',
                    output: '.\\src\\ServerApi.Generated',
                    files: ['..\\WebServer\\bin\\Nimrod.Demo.dll'],
                    verbose: true
                }
            }
        },
        ts: {
            oneFile: {
                // the order of files is important for a one file compilation
                src: ['typings/main/**/*.ts'].concat(customScripts),
                out: '../WebServer/dist/bundle.js',
                options: {
                    target: 'es5',
                    sourceMap: 'true',
                    fast: 'never'
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-nimrod');
    grunt.loadNpmTasks('grunt-ts');
    grunt.registerTask('nimrod-default', ['nimrod']);
};
