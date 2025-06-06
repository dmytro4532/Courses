import { PlusOutlined } from '@ant-design/icons';
import { Form, Input, Button, Space, message, InputNumber, Upload, Select } from 'antd';
import type { UploadFile } from 'antd/es/upload/interface';
import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { Topic, Test } from '../../../types';
import api from '../../../api/axios';
import { fetchTests } from '../../../store/slices/testsSlice';
import type { AppDispatch, RootState } from '../../../store';

interface TopicFormProps {
  courseId: string;
  initialValues?: Topic;
  onSuccess: () => void;
  onCancel: () => void;
  loading?: boolean;
}

interface TopicFormData {
  title: string;
  content: string;
  testId?: string;
  order: number;
}

export const TopicForm = ({ courseId, initialValues, onSuccess, onCancel, loading }: TopicFormProps) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);
  const dispatch = useDispatch<AppDispatch>();
  const { paged: testsPaged } = useSelector((state: RootState) => state.tests);
  const [fileList, setFileList] = useState<UploadFile[]>(
    initialValues?.mediaUrl
      ? [
          {
            uid: '-1',
            name: 'Current Media',
            status: 'done',
            url: initialValues.mediaUrl,
          },
        ]
      : []
  );

  useEffect(() => {
    dispatch(fetchTests({ pageIndex: 0, pageSize: 100 }));
  }, [dispatch]);

  useEffect(() => {
    form.resetFields();
  }, [form, initialValues]);

  const handleSubmit = async (values: TopicFormData) => {
    setSubmitting(true);
    try {
      const formData = new FormData();
      formData.append('title', values.title);
      formData.append('content', values.content);
      formData.append('order', values.order.toString());
      formData.append('courseId', courseId);
      if (values.testId) {
        formData.append('testId', values.testId);
      }

      let response;
      if (initialValues) {
        formData.append('id', initialValues.id);
        response = await api.put(`/api/topics/${initialValues.id}`, formData);

        const mediaFormData = new FormData();
        if (fileList[0]?.originFileObj) {
          mediaFormData.append('media', fileList[0].originFileObj as File);
        }
        mediaFormData.append('topicId', initialValues.id);
        await api.put(`/api/topics/${initialValues.id}/media`, mediaFormData);
      } else {
        response = await api.post('/api/topics', formData);

        if (fileList[0]?.originFileObj && response.data?.id) {
          const mediaFormData = new FormData();
          mediaFormData.append('topicId', response.data.id);
          mediaFormData.append('media', fileList[0].originFileObj as File);
          await api.put(`/api/topics/${response.data.id}/media`, mediaFormData);
        }
      }

      message.success(`Topic ${initialValues ? 'updated' : 'created'} successfully`);
      form.resetFields();
      setFileList([]);
      onSuccess();
    } catch (error: any) {
      message.error(error?.response?.data?.details || `Failed to ${initialValues ? 'update' : 'create'} topic`);
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  const uploadProps = {
    beforeUpload: (file: File) => {
      const isMedia = file.type.startsWith('image/') || file.type.startsWith('video/');
      if (!isMedia) {
        message.error('You can only upload image or video files!');
        return false;
      }
      return false;
    },
    onChange: ({ fileList }: { fileList: UploadFile[] }) => {
      setFileList(fileList);
    },
    fileList,
  };

  return (
    <Form
      form={form}
      layout="vertical"
      onFinish={handleSubmit}
      initialValues={{
        title: initialValues?.title || '',
        content: initialValues?.content || '',
        testId: initialValues?.testId || undefined,
        order: initialValues?.order || 0,
      }}
    >
      <Form.Item
        name="title"
        label="Title"
        rules={[{ required: true, message: 'Please enter the topic title' }]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        name="content"
        label="Content"
        rules={[{ required: true, message: 'Please enter the topic content' }]}
      >
        <Input.TextArea rows={4} />
      </Form.Item>

      <Form.Item
        name="testId"
        label="Test"
      >
        <Select
          allowClear
          placeholder="Select a test"
          options={testsPaged?.items.map((test: Test) => ({
            label: test.title,
            value: test.id,
          }))}
        />
      </Form.Item>

      <Form.Item
        name="order"
        label="Order"
        rules={[{ required: true, message: 'Please enter the topic order' }]}
      >
        <InputNumber min={0} />
      </Form.Item>

      <Form.Item label="Media">
        <Upload
          {...uploadProps}
          maxCount={1}
          listType="picture-card"
        >
          {fileList.length === 0 && (
            <div>
              <PlusOutlined />
              <div style={{ marginTop: 8 }}>Upload</div>
            </div>
          )}
        </Upload>
      </Form.Item>

      <Form.Item>
        <Space>
          <Button
            type="primary"
            htmlType="submit"
            loading={submitting || loading}
          >
            {initialValues ? 'Update' : 'Create'}
          </Button>
          <Button onClick={handleCancel}>
            Cancel
          </Button>
        </Space>
      </Form.Item>
    </Form>
  );
}; 