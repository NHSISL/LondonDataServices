// example_bad_performance.tsx
// Demonstrates VIOLATIONS of performance rules
// ❌ tsr-performance-001 — speculative useMemo with no profiler evidence
// ❌ tsr-performance-002 — large object created inline in render
// ❌ tsr-performance-003 — monolithic component rendering everything
// ❌ tsr-performance-004 — single oversized context causing broad re-renders

import React, { createContext, useContext, useMemo, useState } from 'react';

// ❌ tsr-performance-004 — one context holds unrelated auth + theme + data
type AppState = { userId: string; dark: boolean; students: string[] };
const AppContext = createContext<AppState>({ userId: '', dark: false, students: [] });

const MonolithicPage: React.FC = () => {
  const { students, dark } = useContext(AppContext);
  const [filter, setFilter] = useState('');

  // ❌ tsr-performance-001 — useMemo applied speculatively with no measured problem
  const filtered = useMemo(
    () => students.filter(s => s.toLowerCase().includes(filter.toLowerCase())),
    [students, filter]
  );

  return (
    // ❌ tsr-performance-002 — style object created on every render
    <div style={{ background: dark ? '#000' : '#fff', padding: 16, margin: 8, border: '1px solid #ccc' }}>
      <input value={filter} onChange={e => setFilter(e.target.value)} placeholder="Filter" />
      {/* ❌ tsr-performance-003 — list rendering inline in a large monolithic component */}
      <ul>
        {filtered.map(name => (
          <li key={name} style={{ padding: '4px 8px', fontWeight: 'bold', color: dark ? '#fff' : '#000' }}>
            {name}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default MonolithicPage;
