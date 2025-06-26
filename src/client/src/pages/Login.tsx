import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, Link, useLocation } from 'react-router-dom';
import type { AppDispatch, RootState } from '../store';
import { login, clearErrors } from '../store/slices/authSlice';

const { Title, Text } = Typography;

const Login = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const location = useLocation();
  const { status, loginError: error } = useSelector((state: RootState) => state.auth);

  useEffect(() => {
    dispatch(clearErrors());
  }, [dispatch]);

  const from = location.state?.from?.pathname || '/';

  const onFinish = async (values: { email: string; password: string }) => {
    dispatch(login(values))
      .unwrap()
      .then(() => navigate(from, { replace: true }));
  };

  return (
    <div style={{ maxWidth: 400, margin: '100px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            З поверненням
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Будь ласка, увійдіть, щоб продовжити
          </Text>

          {error && (
            <Alert
              type="error"
              message={error}
              style={{ marginBottom: 0 }}
              showIcon
              closable
            />
          )}

          <Form
            form={form}
            name="login"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Будь ласка, введіть свою електронну адресу' },
                { type: 'email' }
              ]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Електронна пошта"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                { required: true, message: 'Будь ласка, введіть свій пароль' },
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Пароль"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                size="large"
                block
                loading={status === 'loading'}
              >
                {status === 'loading' ? 'Вхід...' : 'Увійти'}
              </Button>
            </Form.Item>
          </Form>

          <div style={{ textAlign: 'center' }}>
            <Text type="secondary">
              Не маєте облікового запису? <Link to="/register">Зареєструватися</Link>
            </Text>
          </div>
        </Space>
      </Card>
    </div>
  );
};

export default Login; 