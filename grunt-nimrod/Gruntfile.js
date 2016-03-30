/*
 * nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 RES
 * Licensed under the MIT license.
 */

'use strict';

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
                    },
                ],
            },
        },
        nodeunit: {
            tests: ['test/*_test.js']
        },
        ts: {
            default: {
                files: {
                    'tasks/': ['src/**/*.ts', 'typings/**/*.ts']
                },
                options: {
                    module: 'commonjs',
                    fast: 'never',
                    outDir: '../tasks/',
                    target: 'es5',
                    sourceMap: 'true',
                    declaration: 'true'
                }
            }
        },
    });

    grunt.loadTasks('tasks');

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-ts');

    grunt.registerTask('default', ['clean', 'ts', 'copy']);

};
