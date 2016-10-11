/// <binding />

module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        nimrod: {
            default_options: {
                options: {
                    // specify the dev executable for the demo
                    output: '.\\src\\Nimrod.Generated',
                    files: ['..\\WebServer\\bin\\Nimrod.Demo.dll'],
                    verbose: true
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-nimrod');
    grunt.registerTask('default', ['nimrod']);
};
