using System;
using System.Collections.Generic;
using System.Linq;
using AppExample.Application.Common.Mappings;
using AppExample.Core.Enums;
using AutoMapper;

namespace AppExample.Application.Feature.Order.Queries.GetOrderDetail
{
    public class OrderDetailsDto : IMapFrom<Core.Entities.Order>
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public float? TotalCost { get; set; }

        /// <summary>
        /// Адресс, откуда надо забрать
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// Адресс, куда надо доставить
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Дата, с
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата, по
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Фактическая дата завершения заказа
        /// </summary>
        public DateTime? DateFinish { get; set; }

        public StatusEnum Status { get; set; }

        /// <summary>
        /// Заказчик (клиент)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Тип услуги
        /// </summary>
        /// <example>Курьерская доставка</example>
        public int ServiceTypeId { get; set; }

        public int? CourierId { get; set; }
        public string Reason { get; set; }
        public IEnumerable<OrderStructureDto> OrderStructures { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Core.Entities.Order, OrderDetailsDto>()
                .ForMember(f => f.OrderStructures,
                    opt
                        => opt.MapFrom(order => order.OrderStructures
                            .Select(structure => new OrderStructureDto
                            {
                                Id = structure.Id,
                                Count = structure.Count,
                                Product = structure.Product,
                                OrderId = structure.OrderId,
                                UnitId = structure.UnitId
                            })));
        }
    }
}