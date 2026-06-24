// example_bad_accessibility.tsx
// Demonstrates VIOLATIONS of accessibility rules
// ❌ tsr-accessibility-001 — div used as button
// ❌ tsr-accessibility-002 — input has no label
// ❌ tsr-accessibility-003 — img missing alt attribute
// ❌ tsr-accessibility-004 — tabIndex removed from interactive element
// ❌ tsr-accessibility-005 — error shown only as hidden tooltip, not as visible text

import React, { useState } from 'react';

type LoginFormProps = {
  onSubmit: (username: string, password: string) => void;
  logoSrc: string;
};

const LoginForm: React.FC<LoginFormProps> = ({ onSubmit, logoSrc }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSave = () => {
    if (!username) {
      setError('Username is required.');
      return;
    }
    onSubmit(username, password);
  };

  return (
    <div>
      {/* ❌ tsr-accessibility-003 — no alt attribute */}
      <img src={logoSrc} />

      {/* ❌ tsr-accessibility-002 — no label, only placeholder */}
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={e => setUsername(e.target.value)}
      />

      {/* ❌ tsr-accessibility-005 — error is a CSS-only tooltip, not visible text */}
      {error && (
        <span title={error} style={{ display: 'none' }}>{error}</span>
      )}

      {/* ❌ tsr-accessibility-002 — password input also unlabeled */}
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={e => setPassword(e.target.value)}
      />

      {/* ❌ tsr-accessibility-001 — div used as interactive button */}
      {/* ❌ tsr-accessibility-004 — tabIndex -1 removes keyboard access */}
      <div
        onClick={handleSave}
        tabIndex={-1}
        style={{ cursor: 'pointer', background: 'blue', color: 'white', padding: '8px' }}
      >
        Sign in
      </div>
    </div>
  );
};

export default LoginForm;
