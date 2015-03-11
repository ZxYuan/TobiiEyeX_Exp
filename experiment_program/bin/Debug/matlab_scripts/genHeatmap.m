function [mymsg]  =  genHeatmap(image_name,fixation_file)


SCREEN_WIDTH = 1280;
SCREEN_HEIGHT = 1024;

SHOW_WIDTH = 960;
SHOW_HEIGHT = 768;

% read stimlui image, get the size
img = imread([image_name, '.png']);
img = imresize(img,[SHOW_HEIGHT SHOW_WIDTH]);
sz = size(img(:,:,1));
width = sz(2);
height = sz(1);


% read track data, cal coordinate, get valid fixations
[idx,st,x,y,fixed]=textread(fixation_file,'%s%s%s%s%s','headerlines',4)
x = str2double(x) - (SCREEN_WIDTH - width) / 2;
y = str2double(y) - (SCREEN_HEIGHT - height) / 2;
fixlen = length(fixed);
fixations = [];
for i=1:fixlen
    if strcmp(fixed(i),'true') && x(i)>0 && x(i)<width && y(i)>0 && y(i)<height
        fixations = [fixations;x(i),y(i)];
    end
end

% gaussian for filter
% GSIZE should be odd
GSIZE = 101;
gfilt = fspecial('gaussian', GSIZE, 23);
halfSample = (GSIZE-1)/2;

% initialize for heatmap
heatmap = zeros(width+2*halfSample, height+2*halfSample);

numErrors = 0;
numFixations = length(fixations);
for i=1:numFixations
    lowerXBound = halfSample+round(fixations(i,1))-halfSample;
    upperXBound = halfSample+round(fixations(i,1))+halfSample;
    lowerYBound = halfSample+round(fixations(i,2))-halfSample;
    upperYBound = halfSample+round(fixations(i,2))+halfSample;
    heatmap(lowerXBound:upperXBound, lowerYBound:upperYBound) = ...
    	heatmap(lowerXBound:upperXBound, lowerYBound:upperYBound) + gfilt;
end
heatmap = heatmap(halfSample+1:width+halfSample,halfSample+1:height+halfSample);
% hack matlab's backwards image display :(
%heatmap = im2uint16(heatmap);
heatmap = im2uint8(heatmap/max(heatmap(:)));
heatmap = heatmap';

imwrite(heatmap,[fixation_file(1:length(fixation_file)-4),'_heatmap.jpg']);
%imagesc(heatmap);


fixationmap = heatmap;
load('map.mat');

%map = colormap(jet);
fixationmap_rgb = uint8(255*ind2rgb(fixationmap, map));
tmp = fixationmap_rgb(:,:,3);
tmp(tmp==143) = 0;
fixationmap_rgb(:,:,3) = tmp;
newimg = fixationmap_rgb + 0.8*img;
%imagesc(newimg);
%pause;
imwrite(newimg, [fixation_file(1:length(fixation_file)-4), '_mapOnImg.jpg']);
mymsg = 'hello world';




