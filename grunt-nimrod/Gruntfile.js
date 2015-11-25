/*
 * nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 Cyril Gandon
 * Licensed under the MIT license.
 */

'use strict';

module.exports = function(grunt) {

  // Project configuration.
  grunt.initConfig({

    // Before generating any new files, remove any previously-created files.
    clean: {
      tests: ['tmp']
    },

    // Configuration to be run (and then tested).
    nimrod: {
      default_options: {
        options: {
			module: 'typescript',
			output: '.\\Views\\ServerApi.Generated',
			files : ['C:\\SoftwareGit\\software\\WikiProject\\Wiki\\bin\\RES.WikiProject.Wiki.dll'
				,'C:\\SoftwareGit\\software\\WikiProject\\Wiki\\bin\\RES.Insee.Wiki.dll']
        },
        files: {
          'tmp/default_options': ['test/fixtures/testing', 'test/fixtures/123']
        }
      }
    },

    // Unit tests.
    nodeunit: {
      tests: ['test/*_test.js']
    }

  });

  // Actually load this plugin's task(s).
  grunt.loadTasks('tasks');

  // These plugins provide necessary tasks.
  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-nodeunit');

  // Whenever the "test" task is run, first clean the "tmp" dir, then run this
  // plugin's task(s), then test the result.
  grunt.registerTask('test', ['clean', 'nimrod', 'nodeunit']);

  // By default, lint and run all tests.
  grunt.registerTask('default', ['nimrod:default_options']);

};
