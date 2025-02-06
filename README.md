# BST-SheetsEditor
**BeStISE** - **Be**at**St**ream **I**nfo **S**heets **E**ditor *(can be read as "besties" lol)*
The more convenient way to edit BeatStream sheet files.

# What the hell is a sheet file?
In BeatStream a lot of extendable data is stored in `csv` files, that contain definitions for many things.
While technically those are human-readable text files, they almost never provide context behind each piece of data (unlike `xml` files, that are used in some other BEMANI games). This tool is meant to simplify editing process of these files.

# Info Sheets
| Name | Location | Contents |
|------|----------|---------|
| Music List | `./sound/musiclist.csv` | Songlist data. |
| BEAST HACKER List | `./others/hacker_list.csv` | BEAST HACKER unlock system definitions. |
| Course Mode List | `./others/courselist.csv` | Course Mode course definitions. |
| BEAST CRISIS List | `data2/others/crysislist.csv` | BEAST CRISIS unlock system definitions. |
| Character List | `data2/others/chara_list.csv` | Aibou (character) definitions. |
| HIGH TENSION Sheet | `data2/sound/ht_sheat.txt` | HIGH TENSION TIME definitions. |
| Particle Motion File | `data2/particle/motionfile/*.pm` | HIGH TENSION TIME particle motion definitions. |
| Standard Stream File / Stream Prefab | `data/stream/stream_*.str` | Standard Stream path definitions. |

> [!NOTE]
> `.` in the beginning of path can be replaced by either `data` or `data2`.
> The actual formats are different between BST1 and BST2 though

> [!NOTE]
> `*` is just a wildcard for "anything"