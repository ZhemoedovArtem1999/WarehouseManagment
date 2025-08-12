import React from 'react';
import { ProgressSpinner } from 'primereact/progressspinner';

interface LoadingSpinnerProps {
  message?: string;
}

const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({ message }) => {
  return (
    <div className={`loading-container`}>
      <div className="loading-content">
        <ProgressSpinner 
          style={{ width: '50px', height: '50px' }} 
          strokeWidth="4"
          animationDuration=".5s"
        />
        {message && <p className="loading-message">{message}</p>}
      </div>
    </div>
  );
};

export default LoadingSpinner;