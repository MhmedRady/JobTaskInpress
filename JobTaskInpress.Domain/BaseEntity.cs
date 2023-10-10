﻿namespace JobTaskInpress.Domain.Entities;

public class BaseEntity<TKey>
{
    public virtual TKey Id { get; protected set; }

}