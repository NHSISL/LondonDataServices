// example_good_performance.tsx
// Demonstrates: no speculative memoisation, stable constants, split context, lazy route
// Rules: tsr-performance-001, 002, 003, 004, 005, 006

import React, { createContext, useContext, useState } from 'react';

// tsr-performance-002 — stable constant defined at module scope, not inside render
const LIST_ITEM_STYLE: React.CSSProperties = { padding: '4px 8px' };

// tsr-performance-004 — contexts split by update frequency
type AuthContextValue = { userId: string };
const AuthContext = createContext<AuthContextValue>({ userId: '' });

type ThemeContextValue = { dark: boolean };
const ThemeContext = createContext<ThemeContextValue>({ dark: false });

// tsr-performance-003 — focused component (only renders list item)
type StudentListItemProps = { name: string };
const StudentListItem: React.FC<StudentListItemProps> = ({ name }) => (
  <li style={LIST_ITEM_STYLE}>{name}</li>
);

// tsr-performance-003 — focused component (only renders list)
type StudentListProps = { students: string[] };
const StudentList: React.FC<StudentListProps> = ({ students }) => (
  <ul>
    {students.map(name => (
      <StudentListItem key={name} name={name} />
    ))}
  </ul>
);

// tsr-performance-001 — no useMemo; filtering is not a measured bottleneck here
const FilteredStudentList: React.FC<{ students: string[]; filter: string }> = ({
  students,
  filter,
}) => {
  const filtered = students.filter(s => s.toLowerCase().includes(filter.toLowerCase()));
  return <StudentList students={filtered} />;
};

// tsr-performance-004 — providers are separate; only relevant consumers re-render
const AppProviders: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [auth] = useState<AuthContextValue>({ userId: 'user-1' });
  const [theme] = useState<ThemeContextValue>({ dark: false });

  return (
    <AuthContext.Provider value={auth}>
      <ThemeContext.Provider value={theme}>
        {children}
      </ThemeContext.Provider>
    </AuthContext.Provider>
  );
};

export { AppProviders, FilteredStudentList };

// tsr-performance-006 — route-level lazy loading (used in router file)
// const StudentPage = React.lazy(() => import('./pages/StudentPage'));
