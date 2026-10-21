# GitHub Copilot Instructions for the "Shortcuts (Continued)" Mod

## Mod Overview and Purpose

The "Shortcuts (Continued)" mod is a continuation of Andreas Pardeike's original "Shortcuts" mod for RimWorld. This mod is designed to enhance the user interface by introducing keyboard shortcuts to facilitate easier navigation and management of pawns and search functionalities within the game. Although the original mod has been retired and is now open for community development, its purpose remains to provide a more streamlined gaming experience by minimizing mouse reliance.

## Key Features and Systems

1. **Pawn Tabs Shortcuts**: 
   - Introduces keyboard shortcuts for quickly accessing different pawn tabs:
     - `i` for Gear
     - `B` for Bio
     - `N` for Needs
     - `H` for Health
   - These shortcuts are configurable in the game's key bindings settings.

2. **Search Dialog Enhancements**:
   - Adds functionality to use the cursor up/down keys within the search dialog.
   - Enter key selects and jumps to the chosen search result row.
   - Recommends changing the vanilla search menu shortcut from `Z` to `Enter` for optimal use.

3. **Mod Compatibility**:
   - Initially, test the mod with only its requirements to isolate potential issues.
   - Use RimSort to manage and sort your mods for better compatibility.

4. **Community Support**:
   - Encourages users to report issues via Discord and the GitHub repository for effective resolution.
   - Promotes community contributions for solutions.

## Coding Patterns and Conventions

- **Naming**: 
  - Follow C# naming conventions; use PascalCase for class names and camelCase for method names.
  - Consistency in naming is essential for maintainability and readability.

- **Structure**:
  - Organize classes logically to reflect their functionality.
  - Maintain a clear separation between UI logic and core functionality.

- **Documentation**:
  - Use XML comments for public methods and complex logic.
  - Maintain a change log for significant updates and patches.

## XML Integration

- **Key Bindings Definition**:
  - XML located in `KeyBindings.xml` defining new key bindings is structured under `KeyBindingCategoryDef` and `KeyBindingDef`.
  - Example snippet:
    xml
    <KeyBindingDef ParentName="Root_KeyBindings">
      <defName>OpenGear</defName>
      <label>Gear</label>
      <category>PawnTabs</category>
      <defaultKeyCodeA>i</defaultKeyCodeA>
    </KeyBindingDef>
    

- **Localizations**:
  - Translation templates are used to ensure the mod is accessible in multiple languages by maintaining separate XML files for key binding translations.

## Harmony Patching

The mod relies on Harmony for method patching, a crucial tool for modders to modify game behavior without altering the core code. The following guidelines are adopted:

- **Postfix and Prefix Patches**:
  - `Postfix` methods are applied to existing game behaviors to extend or modify their functionality after the original method's execution.
  - `Prefix` methods execute before the original method, potentially overriding its execution.

- **Example Usage**:
  - `Dialog_MapSearch_PostClose.cs` and `Dialog_MapSearch_PostOpen.cs` utilize `Postfix` methods to adjust behavior after windows have been opened or closed.

## Suggestions for Copilot

- **Auto-completion**:
  - Use suggestions to streamline coding of repetitive patterns, especially when adding new shortcut keys or UI enhancements.

- **Debugging Assistance**:
  - Copilot can aid in generating boilerplate code for logging to ensure comprehensive error handling and reporting.

- **Localization**:
  - Leverage Copilot to create consistent localization entries across XML files for different languages.

- **Optimization Hints**:
  - Use Copilot’s suggestions to optimize existing methods and potentially identify areas for performance improvements.

By adhering to these instructions, mod developers can effectively contribute to and expand upon the "Shortcuts (Continued)" mod, while maintaining its functionality and ease of use.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

