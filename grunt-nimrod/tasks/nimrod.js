/*
 * grunt-nimrod
 * https://github.com/resgroup/nimrod
 *
 * Copyright (c) 2015 RES
 * Licensed under the MIT license.
 */
 
module.exports = function(grunt) {
  var cp = require('child_process');

  grunt.registerMultiTask('nimrod', 'An ASP.NET MVC to TypeScript Converter', function() {

	  var options = this.options();
	  var cmd = __dirname + '\\Nimrod.Console\\bin\\Release\\Nimrod.Console.exe -m '+ options.module +' -o '+options.output+' --files=' + options.files.join(':');
	  
	  this.async();

    childProcess = cp.exec(cmd);

    childProcess.stdout.on('data', function (d) { grunt.log.write(d); });
    childProcess.stderr.on('data', function (d) { grunt.log.error(d); });

  });
};
