// example_good_accessibility.tsx
// Demonstrates: semantic HTML, labeled inputs, alt text, keyboard nav, accessible errors
// Rule: tsr-accessibility-001, 002, 003, 004, 005

import React, { useState } from 'react';

type LoginFormValues = {
  username: string;
  password: string;
};

type LoginFormErrors = Partial<Record<keyof LoginFormValues, string>>;

type LoginFormProps = {
  onSubmit: (values: LoginFormValues) => void;
  logoSrc: string;
};

const LoginForm: React.FC<LoginFormProps> = ({ onSubmit, logoSrc }) => {
  const [values, setValues] = useState<LoginFormValues>({ username: '', password: '' });
  const [errors, setErrors] = useState<LoginFormErrors>({});

  const validate = (): LoginFormErrors => {
    const errs: LoginFormErrors = {};
    if (!values.username) errs.username = 'Username is required.';
    if (!values.password) errs.password = 'Password is required.';
    return errs;
  };

  const handleSubmit = () => {
    const errs = validate();
    if (Object.keys(errs).length > 0) {
      setErrors(errs);
      return;
    }
    onSubmit(values);
  };

  return (
    <div>
      {/* tsr-accessibility-003 — informative image with descriptive alt text */}
      <img src={logoSrc} alt="Company logo" />

      <form>
        <div>
          {/* tsr-accessibility-002 — label associated via htmlFor */}
          <label htmlFor="username">Username</label>
          {/* tsr-accessibility-005 — aria-describedby links error to input */}
          <input
            id="username"
            type="text"
            value={values.username}
            onChange={e => setValues(v => ({ ...v, username: e.target.value }))}
            aria-describedby={errors.username ? 'username-error' : undefined}
          />
          {errors.username && (
            <p id="username-error" role="alert">{errors.username}</p>
          )}
        </div>

        <div>
          <label htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            value={values.password}
            onChange={e => setValues(v => ({ ...v, password: e.target.value }))}
            aria-describedby={errors.password ? 'password-error' : undefined}
          />
          {errors.password && (
            <p id="password-error" role="alert">{errors.password}</p>
          )}
        </div>

        {/* tsr-accessibility-001 — button element for action (keyboard-focusable) */}
        {/* tsr-accessibility-004 — keyboard nav preserved; no tabIndex removal */}
        <button type="button" onClick={handleSubmit}>
          Sign in
        </button>
      </form>
    </div>
  );
};

export default LoginForm;
