import { LockOutlined, UserOutlined, MailOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../store';
import { register, clearErrors } from '../store/slices/authSlice';

const { Title, Text } = Typography;

const Register = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { status, registerError: error } = useSelector((state: RootState) => state.auth);

  useEffect(() => {
    dispatch(clearErrors());
  }, [dispatch]);

  const onFinish = async (values: { username: string; email: string; password: string }) => {
    dispatch(register(values))
      .unwrap()
      .then(() => navigate('/login'));
  };

  return (
    <div style={{ maxWidth: 400, margin: '100px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            Створити аккаунт
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Зареєструйтесь, щоб почати
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
            name="register"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="username"
              rules={[
                { required: true, message: 'Будь ласка, введіть своє ім\'я користувача' },
                { min: 3, message: 'Ім\'я користувача повинно містити щонайменше 3 символи' }
              ]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Ім'я користувача"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Будь ласка, введіть свою електронну адресу' },
                { type: 'email', message: 'Будь ласка, введіть дійсну електронну адресу' }
              ]}
            >
              <Input
                prefix={<MailOutlined />}
                placeholder="Електронна пошта"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                { required: true, message: 'Будь ласка, введіть свій пароль' },
                { min: 6, message: 'Пароль повинен містити щонайменше 6 символів' }
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Пароль"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="confirmPassword"
              dependencies={['password']}
              rules={[
                { required: true, message: 'Будь ласка, підтвердіть свій пароль' },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error('Два паролі не збігаються'));
                  },
                }),
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Підтвердіть пароль"
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
                {status === 'loading' ? 'Створення облікового запису...' : 'Створити аккаунт'}
              </Button>
            </Form.Item>
          </Form>

          <div style={{ textAlign: 'center' }}>
            <Text type="secondary">
              Вже є аккаунт? <Link to="/login">Увійти</Link>
            </Text>
          </div>
        </Space>
      </Card>
    </div>
  );
};

export default Register; 