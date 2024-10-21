# beastutils
A collection of various utilities for BeatStream modding.

# Utils in this repo
Each utility is placed in its own branch, with `main` acting as a sort of hub to other branches.

|Utility|Description|Release link|
|-------|-----------|-------------|
| [bsthsmc](https://github.com/HiLordReilo/beastutils/tree/bsthsmc) | **B**eat**St**ream **H**it**s**ound **M**od **C**reator. A convenient way to inject custom hitsounds into the game. | [Link](https://github.com/HiLordReilo/beastutils/releases/tag/BST_HitsoundModCreator) |
| [bestise](https://github.com/HiLordReilo/beastutils/tree/bestise) | **Be**at**St**ream **I**nfo **S**heets **E**ditor. A convenient way to edit various csv files found in the game (`musiclist.csv`, `hackerlist.csv`, `courselist.csv` and more). | [Link](https://github.com/HiLordReilo/beastutils/releases/tag/BST_SheetsEditor) |

# Additional utils
These utilities are not a part of this repository, but still are very useful for modding.

|Utility|Description|BST use case|
|-------|-----------|------------|
| [ifs_layeredfs](https://github.com/mon/ifs_layeredfs) by mon | Makes data mods possible without modifying original files. | Keeping original files intact, as well as injection of new small jackets on runtime. |
| [2dxTools](https://github.com/mon/2dxTools) by mon | A set of tools for working with `2dx` audio containers. | Extraction and creation of `2dx` files, used for actual songs. |
| [SD9Tool](https://github.com/TheFooestBar/SD9Tool) by TheFooestBar | A tool to modify `sd9` headers, as well as extract audio from said files. | Because it is honestly not the most convenient tool, I personally only used its source as a reference for `sd9` header format. `sd9` files are used for system sounds. |
| [ifstools](https://github.com/mon/ifstools) by mon | Extractor and repacker for `ifs` files. | Extraction of textures, as well as revealing the file structure inside of one `ifs` archive. Also used to create new ifs archives with song jackets. |
| [BoxStream Editor](https://github.com/48productions/BoxStream-Editor) by 48productions | An actual chart editor. **IT IS STILL IN DEVELOPMENT AND IS SHARED PRIVATELY** | Custom charts creation, as well as BST's `ply` chart format research. |

# Extra Docs
- [Chart Format (.ply) description](https://docs.google.com/document/d/1DZbcXgUmYKdO4SQav00cr0fdi1xcciIUUpHjrbsJipA/)
- [Particle Motion (.pm) file description](https://docs.google.com/document/d/1wBjqLMizdO64gcFrK3_7-2TInUhYNPsg-TTV2YzdoHY/edit?usp=sharing)
