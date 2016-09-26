/*
 * nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2016 RES
 * Licensed under the MIT license.
 */

module.exports = function (grunt) {
    grunt.initConfig({

        // Before generating any new files, remove any previously-created files.
        clean: {
            insideFolder: ['tasks/*']
        },
        copy: {
            default: {
                files: [
                    {
                        expand: false,
                        flatten: true,
                        src: '../Nimrod.Console/bin/Release/**/*',
                        dest: 'tasks/Release/'
                    }
                ]
            }
        },
        nodeunit: {
            tests: ['test/*_test.js']
        },
        ts: {
            default: {
                tsconfig: true
            }
        }
    });

    grunt.loadTasks('tasks');

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-ts');

    grunt.registerTask('default', ['clean', 'ts', 'copy']);

};
