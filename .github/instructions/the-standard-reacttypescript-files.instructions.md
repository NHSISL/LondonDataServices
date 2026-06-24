---
applyTo: "src/**/*"
---

# The Standard React + TypeScript + Vite — Files

## Applies To
All source files in a Standard-compliant React + TypeScript + Vite project — `src/**/*`.

## Rules — Do
- Each file must contain one primary responsibility (tsr-files-001)
- File names must describe architectural role using the defined naming patterns (tsr-files-002)
- React component files must use the `.tsx` extension (tsr-files-003)
- Non-rendering TypeScript files must use the `.ts` extension (tsr-files-004)
- Organize files into the standard project structure by architectural layer (tsr-files-005)

## Rules — Do Not
- Never create generic `utils`, `helpers`, or `common` files (tsr-files-005)
- Never mix multiple architectural responsibilities in one file (tsr-files-006)
- Never use `.tsx` for non-rendering files (tsr-files-007)

## Defaults
- When a file exports one class or function representing an architectural role, its name follows the role naming pattern.
- When a new domain concept is introduced, create a dedicated folder under the appropriate layer.
