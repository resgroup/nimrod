/*
 * nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 RES
 * Licensed under the MIT license.
 */

'use strict';

module.exports = function(grunt) {

  // Project configuration.
  grunt.initConfig({

    // Before generating any new files, remove any previously-created files.
    clean: {
      insideFolder: ['tasks/Nimrod.Console/*'],
	  folder: ['tasks/Nimrod.Console']
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
    // Configuration to be run (and then tested).
    nimrod: {
      default_options: {
        options: {
			module: 'typescript',
			output: 'C:\\temp\\nimrod-test-generated',
			files : ['C:\\SoftwareGit\\software\\WikiProject\\Wiki\\bin\\RES.WikiProject.Wiki.dll'
				,'C:\\SoftwareGit\\software\\WikiProject\\Wiki\\bin\\RES.Insee.Wiki.dll']
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

  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-contrib-copy');

  grunt.registerTask('default', ['clean', 'copy:default', 'nimrod:default_options']);
  
  grunt.registerTask('publish', ['clean', 'copy:default']);

};
