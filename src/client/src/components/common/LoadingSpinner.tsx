import { Spin } from 'antd';

interface LoadingSpinnerProps {
  size?: 'small' | 'default' | 'large';
}

const LoadingSpinner = ({ size = 'large' }: LoadingSpinnerProps) => {
  return (
    <div style={{ textAlign: 'center', padding: '50px' }}>
      <Spin size={size} />
    </div>
  );
};

export default LoadingSpinner; 