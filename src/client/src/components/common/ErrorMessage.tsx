import { Typography } from 'antd';

const { Title } = Typography;

interface ErrorMessageProps {
  message: string;
  level?: 1 | 2 | 3 | 4 | 5;
}

const ErrorMessage = ({ message, level = 3 }: ErrorMessageProps) => {
  return (
    <div style={{ textAlign: 'center', padding: '50px' }}>
      <Title level={level}>Error: {message}</Title>
    </div>
  );
};

export default ErrorMessage; 