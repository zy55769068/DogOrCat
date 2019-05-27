# DogOrCat
DogOrCat Xamarin.Forms With customvision AI

This is a Xamarin.Forms Project

Working with Microsoft CustomVision AI

To tell if a picture is a dog or a cat.

The articles are in [This blog](https://zy55769068.top/2019/05/23/Xamarin.Forms-With-customvision-ai/)


如果编译iOS项目时报错resource fork, Finder information, or similar detritus not allowed.

切换到iOS项目，命令行执行 find . -type f -name '*.mlmodel' -exec xattr -c {} \
