/*
 * nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 Cyril Gandon
 * Licensed under the MIT license.
 */

'use strict';

var cp = require('child_process');

module.exports = function(grunt) {

  // Please see the Grunt documentation for more information regarding task
  // creation: http://gruntjs.com/creating-tasks

  grunt.registerMultiTask('nimrod', 'An ASP.NET MVC to TypeScript Converter', function() {
	  
	  var options = this.options();
	  console.log();
	  var cmd = 'tasks\\Release\\Nimrod.Console.exe -m '+ options.module +' -o '+options.output+' --files=' + options.files.join(':');
		var childProcess = cp.exec(
		cmd,
			function(err, stdout, stderr) { 
				console.log('yo');
				
				console.log(stdout); 
			}
		);
		
    childProcess.stdout.on('data', function (d) {  grunt.log.write(d); });
    childProcess.stderr.on('data', function (d) {  grunt.log.error(d); });
	  
    // Merge task-specific and/or target-specific options with these defaults.
    var options = this.options({
      punctuation: '.',
      separator: ', '
    });

    // Iterate over all specified file groups.
    this.files.forEach(function(f) {
      // Concat specified files.
      var src = f.src.filter(function(filepath) {
        // Warn on and remove invalid source files (if nonull was set).
        if (!grunt.file.exists(filepath)) {
          grunt.log.warn('Source file "' + filepath + '" not found.');
          return false;
        } else {
          return true;
        }
      }).map(function(filepath) {
        // Read file source.
        return grunt.file.read(filepath);
      }).join(grunt.util.normalizelf(options.separator));

      // Handle options.
      src += options.punctuation;

      // Write the destination file.
      //grunt.file.write(f.dest, src);

      // Print a success message.
      //grunt.log.writeln('File "' + f.dest + '" created.');
    });
  });

};
