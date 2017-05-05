'use strict';

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    htmlmin = require('gulp-htmlmin'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    merge = require('merge-stream'),
    del = require('del'),
    runSequence = require('run-sequence'),
    bundleconfig = require('./bundleconfig.json');

var regex = {
    sass: /\.scss$/,
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

gulp.task('transpile', ['transpile:css']);
gulp.task('min', ['min:js', 'min:css']);

gulp.task('default', function (callback) {
    runSequence('transpile', 'min', callback);
});

gulp.task('transpile:css', function () {
    var tasks = getTranspilationBundles(regex.sass).map(function (bundle) {
        return gulp.src(bundle.inputFiles)
            .pipe(sass().on('error', sass.logError))
            .pipe(gulp.dest(bundle.outputPath));
    });
    return merge(tasks);
});

gulp.task('min:js', function () {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
});

gulp.task('min:css', function () {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: '.' })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest('.'));
    });
    return merge(tasks);
});

gulp.task('clean', function () {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    return del(files);
});

gulp.task('watch', function () {
    getBundles(regex.js).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ['min:js']);
    });

    getBundles(regex.css).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ['min:css']);
    });

    getTranspilationBundles(regex.sass).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ['transpile:css']);
    });
});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}

function getTranspilationBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return bundle.inputFiles.some(function (path) { return regexPattern.test(path); });
    });
}