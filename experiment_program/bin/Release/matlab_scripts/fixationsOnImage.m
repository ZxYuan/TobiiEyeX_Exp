function [ output_args ] = fixationsOnImage( img, fixationmap )
    
    map = colormap(jet);
    fixationmap_rgb = uint8(255*ind2rgb(fixationmap, map));
    tmp = fixationmap_rgb(:,:,3);
    tmp(tmp==143) = 0;
    fixationmap_rgb(:,:,3) = tmp;
    newimg = fixationmap_rgb + 0.8*img;
    imagesc(newimg);

end