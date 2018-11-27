/// <binding BeforeBuild='default' AfterBuild='default' />
const gulp = require('gulp');
const concat = require('gulp-concat');
const sass = require("gulp-sass");

const vendorStyles = [
    "node_modules/bootstrap/dist/css/bootstrap.min.css",
    "node_modules/open-iconic/font/css/open-iconic-bootstrap.css",
    "node_modules/bootswatch/dist/flatly/bootstrap.min.css"
];
const vendorScripts = [
    "node_modules/jquery/dist/jquery.min.js",
    "node_modules/popper.js/dist/umd/popper.min.js",
    "node_modules/bootstrap/dist/js/bootstrap.min.js",
];

gulp.task('default', ['build-vendor']);

gulp.task('build-vendor', ['build-vendor-css', 'build-vendor-js', 'copy-openiconic', 'sass']);

gulp.task('build-vendor-css', () => {
    return gulp.src(vendorStyles)
        .pipe(concat('vendor.min.css'))
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('build-vendor-js', () => {
    return gulp.src(vendorScripts)
        .pipe(concat('vendor.min.js'))
        .pipe(gulp.dest('wwwroot/js'));
});

gulp.task('copy-openiconic', () => {
    return gulp.src(['./node_modules/open-iconic/font/fonts/**'])
        .pipe(gulp.dest('./wwwroot/fonts'));
})

gulp.task('sass', () => {
    return gulp.src('./Styles/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('wwwroot/css'))
})