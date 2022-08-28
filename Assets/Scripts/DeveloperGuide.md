# Doodle's Diary Developer Guide

## Introduction
This is the developer guide for Doodle's Diary. It is intended to be a reference for developers and designers for a better understanding of the system and to fasten the process of developing new features.

## Purpose
- To define the system's design and architecture.
- To define the content creation process.
- To define the coding standards.

---

## Development Set Up
- Current Unity Version: [2020.3.27f1](https://unity3d.com/unity/whats-new/2020.3.27)

## Coding Standards

- Developers are to employ the [C# at Google Style Guide](https://google.github.io/styleguide/csharp-style.html) for all source code and documentation.

## System Architecture

### Tower

### Player

### Enemy

### Map

### UI

### Menu


## Branch Management
- `main` branch: For stable releases. Only merge into this branch when the build is stable and ready for release.
- `staging` branch: For development and testing. Merge into this branch to resolve conflicts and to test new features.
- All other branches:
  - Naming convention: `<Category>-<Task>`.
    - Category:
      - `FeatDev`: Feature Developement;
      - `Test`: For independent testing.
      - `CodeRe`: Code refactor, clean or comment (Isolated refactoring involving independent code);
      - `ProjRestruc`: Code base restructuring (Massive refactoring involving multi-depentency);
      - `Debug`: Bug fix;
    - Task:
      - Keywords that define your purpose of creating the branch.
  - Branches that are not in used or already merged into the `main` branch are to be **deleted**.

## Code Review Process

## Content Creation Flow

