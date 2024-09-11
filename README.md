# BST_HitsoundModCreator
An utility to create Hitsound mods for BeatStream.
Works both for BST1 and BST2.

# Usage
`tbsthsmc.exe <soundset> <destination> <sound(s)>`

`<soundset>` - which hitsound set should be replaced. Can be 0, 1, 2 or "default", "clap", "tambourine" respectively.
`<destination>` - name of the folder where new hitsounds will be placed. If not specified and you passed in multiple sounds, you will be prompted to specify the location.
`<sound(-s)>` - properly named Microsoft ADPCM formatted WAV file. It's possible to pass in multiple sounds at once, just separate filenames by spaces.

The proper names are:

- tap.wav/basic.wav/note.wav
- slash.wav/flick.wav
- ripple.wav
- stream.wav

> [!TIP]
> Alternatively, you can just drag and drop your sounds onto bsthsmc.exe.
> If destination is not specified/empty, files will be created in the same directory as bsthsmc.exe without proper directory tree.
> If name of the sound doesn't match templates, you will be prompted to select hitsound type.
> If soundset is not specified, you will be prompted to choose a soundset to replace.

# Usage (result)
Just drop the resulting folder in your `data_mods` folder and you're golden.